using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;
using AuthorizationServer.Services;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.ApiResponse;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AuthorizationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("external")]
  public class ExternalAuthController : Controller
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly AuthorizationUnitOfWork _db;

    private readonly FacebookAuthSettings _fbAuthSettings;

    private readonly IJwtService _jwtService;
    private readonly VkAuthSettings _vkAuthSettings;


    public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
      IOptions<VkAuthSettings> vkAuthSettingsAccessor, IJwtService jwtService, AuthorizationUnitOfWork db)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      _jwtService = jwtService;
      _db = db;
    }

    //https://habr.com/post/270273/ - lets encrypt


    [HttpPost("vkLogin")]
    public async Task<IActionResult> VkLogin([FromBody] VkAuthCodeViewModel vkAuth)
    {
      if (vkAuth == null)
        return new BadResponseResult("Input body is null.");
      if (!ModelState.IsValid)
        return new BadResponseResult("Validation error.", ModelState);
      if (vkAuth.State.IsNullOrEmpty() || vkAuth.State != "123456")
        return new BadResponseResult("You are hacker!");
      if (!vkAuth.Status || !vkAuth.Error.IsNullOrEmpty())
        return new BadResponseResult(vkAuth.Error + " " + vkAuth.ErrorDescription);
      if (string.IsNullOrEmpty(vkAuth.Code))
        return new BadResponseResult("Code is null or empty.");

      //var appAccessTokenResponse =await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=http://localhost:6210/vk-auth-code.html&code={vkAuth.Code}");
      var appAccessTokenResponse =await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=https://2buttons.ru/vk-auth-code.html&code={vkAuth.Code}");
      var appAccessToken = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);


      var user = await GetOrCreateUserAccountFromVk(appAccessToken.UserId, appAccessToken.AccessToken,
        appAccessToken.Email);
      if (user == null)
        return new BadResponseResult("We can not find or create you.");

      var accessToken =  await AccessToken(user);
      

    }

    private bool IsFullAccount(userMain user)
    {
      return user.lo
    }

    private async Task<UserDto> GetOrCreateUserAccountFromVk(int externalUserId, string externalToken, string email)
    {
      var user = await _db.Users.GetUserByExternalUserIdAsync(externalUserId, SocialNetType.Facebook);
      if (user != null)
        return user;

      user = await _db.Users.GetUserByEmail(email);
      if (user != null)
      {
        user.FacebookId = externalUserId;
        user.FacebookToken = externalToken;
        await _db.Users.AddOrChangeExternalUserIdAsync(user.UserId, user.FacebookId, SocialNetType.Facebook,
          user.FacebookToken);

        if (!email.IsNullOrEmpty() && (user.Email.IsNullOrEmpty() || !user.EmailConfirmed))
        {
          if (user.Email == email)
            user.EmailConfirmed = true;

          await _db.Users.ChangeUserEmail(user.UserId, user.Email, user.EmailConfirmed);
        }
        return user;
      }


      var userInfoResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/users.get?user_ids={externalUserId}&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
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
      await _db.Users.AddUserIntoAccountDbAsync(userDb);

      var bdate = Convert.ToDateTime(userInfo.Birthday);

      var links = await UploadAvatars(userDb.UserId, userInfo.SmallPhoto, userInfo.FullPhoto);
      var userMain = new UserMainDb
      {
        UserId = userDb.UserId,
        Login = userInfo.FirstName + " " + userInfo.LastName,
        BirthDate = bdate,
        Sex = userInfo.Sex,
        City = await cityName ?? userInfo.City.Title,
        Description = "",
        SmallAvatarLink = links.Item1,
        FullAvatarLink = links.Item2
      };

      await _db.Users.AddUserIntoMainDbAsync(userMain);

      
      return userDb.ToUserDto();
    }

    private async Task<(string, string)> UploadAvatars(int userId, string smallPhotoUrl, string fullPhotoUrl)
    {
      var jsonSmall = JsonConvert.SerializeObject(new {userId, size = 0, url = smallPhotoUrl});
      var jsonFull = JsonConvert.SerializeObject(new {userId, size = 1, url = fullPhotoUrl});
      var s = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonSmall);
      var f = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonFull);

      await Task.WhenAll(f, s);
      return (f.Result, s.Result);
    }


    [HttpPost("fbLogin")]
    public async Task<IActionResult> Facebook([FromBody] FacebookAuthViewModel fbAuth)
    {
      if (fbAuth == null)
        return new BadResponseResult("Input parameters is null.");
      if (!ModelState.IsValid)
        return new BadResponseResult("Validation error.", ModelState);
      if (fbAuth.State != "123456")
        return new BadResponseResult("You are hacker!");
      if (!fbAuth.Status || !fbAuth.Error.IsNullOrEmpty())
        return new BadResponseResult(fbAuth.Error + " " + fbAuth.ErrorDescription);
      if (string.IsNullOrEmpty(fbAuth.Code))
        return new BadResponseResult("Code is null or empty.");

      // 1.generate an app access token
      var appAccessTokenResponse =
        await Client.GetStringAsync(
          $"https://graph.facebook.com/v3.0/oauth/access_token?client_id={_fbAuthSettings.AppId}&redirect_uri=https://2buttons.ru/facebook-auth-code.html&client_secret={_fbAuthSettings.AppSecret}&code={fbAuth.Code}");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      if (!appAccessToken.Error.IsNullOrEmpty())
        return new BadResponseResult(fbAuth.Error + " " + fbAuth.ErrorDescription);

      var user = await GetOrCreateUserAccountFromFb(appAccessToken.AccessToken);
      if (user == null)
        return new BadResponseResult("We can not find or create account.");

      return await AccessToken(user);
    }


    private async Task<UserDto> GetOrCreateUserAccountFromFb(string externalToken)
    {
      var userInfoResponse = await Client.GetStringAsync(
        $"https://graph.facebook.com/v3.0/me?fields=id,email,first_name,last_name,name,gender,locale,hometown,birthday,picture&access_token={externalToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserResponse>(userInfoResponse);


      var user = await _db.Users.GetUserByExternalUserIdAsync(userInfo.ExternalUserId, SocialNetType.Facebook);
      if (user != null)
        return user;
      user = await _db.Users.GetUserByEmail(userInfo.Email);
      if (user != null)
      {
        user.FacebookId = userInfo.ExternalUserId;
        user.FacebookToken = externalToken;
        await _db.Users.AddOrChangeExternalUserIdAsync(user.UserId, user.FacebookId, SocialNetType.Facebook,
          user.FacebookToken);

        if (!userInfo.Email.IsNullOrEmpty() && (user.Email.IsNullOrEmpty() || !user.EmailConfirmed))
        {
          if (user.Email == userInfo.Email)
            user.EmailConfirmed = true;

          await _db.Users.ChangeUserEmail(user.UserId, user.Email, user.EmailConfirmed);
        }
        return user;
      }

      var userDb = new UserDb
      {
        RoleType = RoleType.User,
        Email = userInfo.Email,
        EmailConfirmed = !string.IsNullOrEmpty(userInfo.Email),
        FacebookId = userInfo.ExternalUserId,
        FacebookToken = externalToken,
        RegistrationDate = DateTime.UtcNow
      };
      await _db.Users.AddUserIntoAccountDbAsync(userDb);

      var bdate = Convert.ToDateTime(userInfo.Birthday);

      var smallUrlResponse = await Client.GetStringAsync(
        $"https://graph.facebook.com/v3.0/{userInfo.ExternalUserId}/picture?type=normal");
      var smallUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(smallUrlResponse).Response.Url;

      var largeUrlResponse = await Client.GetStringAsync(
        $"https://graph.facebook.com/v3.0/{userInfo.ExternalUserId}/picture?type=large");
      var largeUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(largeUrlResponse).Response.Url;
      var links = await UploadAvatars(userDb.UserId, smallUrl, largeUrl);

      await _db.Users.AddUserIntoMainDbAsync(userDb.UserId, userInfo.FirstName + " " + userInfo.LastName, bdate,
        userInfo.SexType, userInfo.City?.Title?.Split(',').FirstOrDefault(), "", links.Item1, links.Item2);


      return userDb.ToUserDto();
    }

    private async Task<Token> AccessToken(UserDto user)
    {
      if (await _db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
      {
        await _db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
        throw new Exception("Your login at leat on 10 defferent devices. We are forced to save your data. Now you are out of all devices. Please log in again.");
      }


      var result = await _jwtService.GenerateJwtAsync(user.UserId, user.RoleType);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");

      return result;
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
      WebRequest request =
        WebRequest.CreateHttp(
          $"https://api.vk.com/method/database.getCitiesById?city_ids={cityId}&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      request.Headers.Add("Cookie", $"remixlang={language}");
      var response = await request.GetResponseAsync();
      using (var stream = response.GetResponseStream())
      {
        using (var reader = new StreamReader(stream))
        {
          data = reader.ReadToEnd();
        }
      }
      response.Close();
      return JsonConvert.DeserializeObject<VkCityResponse>(data).Response.FirstOrDefault().Title;
    }
  }
}