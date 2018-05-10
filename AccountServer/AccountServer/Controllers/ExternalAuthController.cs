using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    private JwtIssuerOptions _jwtOptions;

    private IJwtFactory _jwtFactory;
    //repository to handler the sqlite database




    private TwoButtonsContext _dbMain;

    private AccountUnitOfWork _accountDb;
    //  private static readonly HttpClient Client = new HttpClient();



    private readonly FacebookAuthSettings _fbAuthSettings;
    private readonly VkAuthSettings _vkAuthSettings;
    //private readonly IJwtFactory _jwtFactory;
    //private readonly JwtIssuerOptions _jwtOptions;
    private static readonly HttpClient Client = new HttpClient();

    public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor, IOptions<VkAuthSettings> vkAuthSettingsAccessor, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, AccountUnitOfWork accountDb)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      //_userManager = userManager;
      //_appDbContext = appDbContext;
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;
      _accountDb = accountDb;
    }

    private void ImlicitFlow()
    {
      //var appAccessTokenResponse = await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=https://oauth.vk.com/blank.html&code={model.Code}");
      //var appAccessToken = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);
    }



    [HttpPost("vkLogin")]
    public async Task<IActionResult> VkLogin([FromBody]VkAuthThokenViewModel vkAuth)
    {
      if (vkAuth == null)
        return BadRequest("Input parameters is null.");
      if (!vkAuth.Status || !string.IsNullOrEmpty(vkAuth.Error))
        return BadRequest(vkAuth.Error + " " + vkAuth.ErrorDescription);
      if (string.IsNullOrEmpty(vkAuth.AccessToken))
        return BadRequest("AccessToken is null or empty.");



      //var appAccessTokenResponse = await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=https://oauth.vk.com/blank.html&code={model.Code}");
      //var appAccessToken = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);
      //// 2. validate the user access token
      //var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
      //var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      var user = await _accountDb.Users.GetUserByExternalUserIdAsync(vkAuth.UserId, SocialNetType.Vk);
      if (user != null)
      {
        if (string.IsNullOrEmpty(vkAuth.Email) && user.Email != vkAuth.Email && (string.IsNullOrEmpty(user.Email) || !user.EmailConfirmed))
        {
          user.Email = vkAuth.Email;
          user.EmailConfirmed = true;
          await _accountDb.Users.ChangeUserEmail(user.UserId, user.Email, user.EmailConfirmed);
        }

        return await AccessToken(user);
      }

      return await AccessToken(await CreateUserAccountFromVk(vkAuth.UserId, vkAuth.AccessToken, vkAuth.Email));

      //// 3. we've got a valid token so we can request user data from fb



      ////   model.AccessToken = JsonConvert.DeserializeObject<VkAppAccessTokenCode>(z).AccessToken;
      ////    model.UserId = JsonConvert.DeserializeObject<VkAppAccessTokenCode>(z).UserId;
      //var vk = new VkMethods(vkAuth.AccessToken);
      ////var info = await vk.GetAccountInfo();

      //var friends = await vk.GetFriends(vkAuth.UserId);
      //return Ok();
      //// 1.generate an app access token
      //var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
      //var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      //// 2. validate the user access token
      //var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={vkAuth.AccessToken}&access_token={appAccessToken.AccessToken}");
      //var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      //if (!userAccessTokenValidation.Data.IsValid)
      //{
      //  return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
      //}

      //// 3. we've got a valid token so we can request user data from fb
      //var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={vkAuth.AccessToken}");
      //var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

      //var p = 5;
      //// 4. ready to create the local user account (if necessary) and jwt
      //// var user = await _userManager.FindByEmailAsync(userInfo.Email);

      ////if (user == null)
      ////{
      ////  var appUser = new AppUser
      ////  {
      ////    FirstName = userInfo.FirstName,
      ////    LastName = userInfo.LastName,
      ////    FacebookId = userInfo.Id,
      ////    Email = userInfo.Email,
      ////    UserName = userInfo.Email,
      ////    PictureUrl = userInfo.Picture.Data.Url
      ////  };

      ////  var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

      ////  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      ////  await _appDbContext.Customers.AddAsync(new Customer { IdentityId = appUser.Id, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
      ////  await _appDbContext.SaveChangesAsync();
      ////}

      ////// generate the jwt for the local user...
      ////var localUser = await _userManager.FindByNameAsync(userInfo.Email);

      ////if (localUser == null)
      ////{
      ////  return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
      ////}

      ////var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory, localUser.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

      //return new OkObjectResult("Good");
    }

    private async Task<UserDto> CreateUserAccountFromVk(int externalUserId, string externalToken, string email)
    {

      Task<string> userInfoResponse =  Client.GetStringAsync($"https://api.vk.com/method/account.getProfileInfo&access_token={externalToken}&v=5.74");
      

      Task<string> userPhotosInfoResponse =  Client.GetStringAsync($"https://api.vk.com/method/users.get?user_ids={externalUserId}&fields=photo_100,photo_max_orig&access_token={userAccessToken}&v=5.74");

      await Task.WhenAll(userInfoResponse, userPhotosInfoResponse);

      var userInfo = JsonConvert.DeserializeObject<AccountGetProfileInfoResponse>(userInfoResponse.Result).Response;
      var userInfoPhotos = JsonConvert.DeserializeObject<UsersGetResponse>(userInfoResponse.Result).Response;

     // AccountWrapper.TryAddUser(_dbMain,)

      var userDb = new UserDb
      {
        RoleType = RoleType.User,
        Email = email,
        EmailConfirmed = !string.IsNullOrEmpty(email)
      };

      userDb.FacebookId = externalUserId;
      userDb.FacebookToken = externalToken;

      await _accountDb.Users.AddUserAsync(userDb);
      return userDb.ToUserDto();

    }

    private static async Task DownloadFileAsync(string uriFrom, string uriTo)
    {
      var client = new WebClient();
      await client.DownloadFileTaskAsync(new Uri(uriFrom), uriTo);
    }


    [HttpPost("facebook")]
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

      var p = 5;
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
      var expiresAccessTokenInTime = 60 * 24 * 7 * 4 * 6; // half a week


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
  }
}

