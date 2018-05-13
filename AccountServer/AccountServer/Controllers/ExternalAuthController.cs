using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.Models.Facebook;
using AccountServer.Models.Vk;
using AccountServer.SocialNets;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.InputParameters.Auth;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsAccountDatabase.Entities;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Route("external")]
  public class ExternalAuthController : Controller
  {
    //some config in the appsettings.json
    private readonly JwtIssuerOptions _jwtOptions;

    private readonly IJwtFactory _jwtFactory;
    //repository to handler the sqlite database

    private readonly TwoButtonsContext _dbMain;
    private readonly AccountUnitOfWork _accountDb;
    private readonly FacebookAuthSettings _fbAuthSettings;
    private readonly VkAuthSettings _vkAuthSettings;
    private static readonly HttpClient Client = new HttpClient();

    public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor, IOptions<VkAuthSettings> vkAuthSettingsAccessor, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,TwoButtonsContext buttonsContext, AccountUnitOfWork accountDb)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;
      _accountDb = accountDb;
      _dbMain = buttonsContext;
    }

    //https://habr.com/post/270273/ - lets encrypt


    [HttpPost("vkLogin")]
    public async Task<IActionResult> VkLogin([FromBody]VkAuthCodeViewModel vkAuth)
    {
      if (vkAuth == null)
        return BadRequest("Input parameters is null.");
      if (!vkAuth.Status || !string.IsNullOrEmpty(vkAuth.Error))
        return BadRequest(vkAuth.Error + " " + vkAuth.ErrorDescription);
      if (string.IsNullOrEmpty(vkAuth.Code))
        return BadRequest("AccessToken is null or empty.");

      var appAccessTokenResponse = await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=http://localhost:6256/vk-auth-code.html&code={vkAuth.Code}");
      var appAccessToken = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);

      var user = await _accountDb.Users.GetUserByExternalUserIdAsync(appAccessToken.UserId, SocialNetType.Vk);
      if (user != null)
      {
        if (string.IsNullOrEmpty(appAccessToken.Email) && user.Email != appAccessToken.Email && (string.IsNullOrEmpty(user.Email) || !user.EmailConfirmed))
        {
          user.Email = appAccessToken.Email;
          user.EmailConfirmed = true;
          await _accountDb.Users.ChangeUserEmail(user.UserId, user.Email, user.EmailConfirmed);
        }

        return await AccessToken(user);
      }

      var newUser = await CreateUserAccountFromVk(appAccessToken.UserId, appAccessToken.AccessToken,
        appAccessToken.Email);
      if (newUser == null)
        return BadRequest("We can not create you.");

      return await AccessToken(newUser);

    }

    private async Task<UserDto> CreateUserAccountFromVk(int externalUserId, string externalToken, string email)
    {

      var userInfoResponse = await Client.GetStringAsync($"https://api.vk.com/method/users.get?user_ids={externalUserId}&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
      var userInfo = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.FirstOrDefault();
      var cityName = GetCityNameByIdFromVk(userInfo.City.CityId, LanguageType.Русский);

      var userDb = new UserDb
      {
        RoleType = RoleType.User,
        Email = email,
        EmailConfirmed = !string.IsNullOrEmpty(email),
        VkId = externalUserId,
        VkToken = externalToken,
        RegistrationDate = DateTime.UtcNow
      };
      await _accountDb.Users.AddUserAsync(userDb);

      var bdate = Convert.ToDateTime(userInfo.Birthday);

      var links = await UploadAvatars(userDb.UserId, userInfo.SmallPhoto, userInfo.FullPhoto);
      AccountWrapper.TryAddUser(_dbMain, userDb.UserId, userInfo.FirstName + " " + userInfo.LastName, bdate, userInfo.Sex,
        (await cityName) ?? userInfo.City.Title, "", links.Item1, links.Item2);


      return userDb.ToUserDto();

    }

    private async Task<(string, string)> UploadAvatars(int userId, string smallPhotoUrl, string fullPhotoUrl)
    {
      var jsonSmall = JsonConvert.SerializeObject(new { userId=userId, size = 0, url = smallPhotoUrl });
      var jsonFull = JsonConvert.SerializeObject(new { userId=userId, size = 1, url = fullPhotoUrl });
      var s = UploadPhotoViaLink("http://localhost:6257/images/uploadUserAvatarViaLink", jsonSmall);
      var f = UploadPhotoViaLink("http://localhost:6257/images/uploadUserAvatarViaLink", jsonFull);

      await Task.WhenAll(f, s);
      return (f.Result, s.Result);
    }



    [HttpPost("fbLogin")]
    public async Task<IActionResult> Facebook([FromBody]FacebookAuthViewModel model)
    {
      // 1.generate an app access token
      var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      // 2. validate the user access token
      var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
      var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      if (!userAccessTokenValidation.Data.IsValid)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
      }

      // 3. we've got a valid token so we can request user data from fb
      var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

      // 4. ready to create the local user account (if necessary) and jwt
      // var user = await _userManager.FindByEmailAsync(userInfo.Email);

      //if (user == null)
      //{
      //  var appUser = new AppUser
      //  {
      //    FirstName = userInfo.FirstName,
      //    LastName = userInfo.LastName,
      //    FacebookId = userInfo.Id,
      //    Email = userInfo.Email,
      //    UserName = userInfo.Email,
      //    PictureUrl = userInfo.Picture.Data.Url
      //  };

      //  var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

      //  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      //  await _appDbContext.Customers.AddAsync(new Customer { IdentityId = appUser.Id, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
      //  await _appDbContext.SaveChangesAsync();
      //}

      //// generate the jwt for the local user...
      //var localUser = await _userManager.FindByNameAsync(userInfo.Email);

      //if (localUser == null)
      //{
      //  return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
      //}

      //var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory, localUser.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

      return new OkObjectResult("Good");
    }

    private async Task<IActionResult> AccessToken(UserDto user)
    {
      var nowTime = DateTime.UtcNow;
      const int expiresAccessTokenInTime = 60 * 24 * 7 * 4 * 6; // half a week

      ClientDb client = null;

      client = new ClientDb
      {
        Secret = Guid.NewGuid().ToString(),
        IsActive = true,
        RefreshTokenLifeTime = expiresAccessTokenInTime
      };
      await _accountDb.Clients.AddClientAsync(client);

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = user.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      if (!await _accountDb.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");

      _jwtOptions.ValidFor = TimeSpan.FromMinutes(expiresAccessTokenInTime);
      return Ok(Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, user.RoleType,
        _jwtOptions));
    }

    private async Task<IActionResult> RefreshToken(CredentialsViewModel credentials)
    {
      var nowTime = DateTime.UtcNow;

      if (string.IsNullOrEmpty(credentials.RefreshToken) || string.IsNullOrEmpty(credentials.SecretKey))
        return BadRequest("RefreshToken or SecretKey is null or empty. Please, send again.");

      var client = await _accountDb.Clients.FindClientAsync(credentials.ClientId, credentials.SecretKey);
      if (client == null || !client.IsActive)
        return BadRequest(
          "Sorry, you have not loge in yet or your connection with authorization server is expired. Plese, get access token again");

      var oldToken = await _accountDb.Tokens.FindTokenAsync(credentials.ClientId, credentials.RefreshToken);
      if (oldToken == null)
        return BadRequest("Sorry, we can not find your \"refresh token\". Plese, get access token again.");

      var role = RoleType.Guest;
      if (oldToken.UserId != 0)
        role = await _accountDb.Users.GetUserRoleAsync(oldToken.UserId);

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = oldToken.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      var isDeleted = await _accountDb.Tokens.RemoveTokenAsync(oldToken);
      var isAdded = await _accountDb.Tokens.AddTokenAsync(token);

      if (!isDeleted || !isAdded)
        return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");

      _jwtOptions.ValidFor = TimeSpan.FromMinutes(client.RefreshTokenLifeTime);
      return Ok(Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role,
        _jwtOptions));
    }

    private static async Task<string> UploadPhotoViaLink(string url, string requestJson)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(requestJson);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return reader.ReadToEnd();
      }
    }

    public async Task<string> GetCityNameByIdFromVk(int cityId, LanguageType language)
    {
      string data;
      WebRequest request = WebRequest.CreateHttp($"https://api.vk.com/method/database.getCitiesById?city_ids={cityId}&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      request.Headers.Add("Cookie", $"remixlang={language}");
      WebResponse response = await request.GetResponseAsync();
      using (Stream stream = response.GetResponseStream())
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          data =reader.ReadToEnd();
        }
      }
      response.Close();
      return JsonConvert.DeserializeObject<VkCityResponse>(data).Response.FirstOrDefault().Title;
    }
  }
}

