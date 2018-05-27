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


    [HttpPost("externalLogin")]
    public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginViewModel auth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (auth.State != "123456")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorret");
        return new BadResponseResult(ModelState);
      }
      if (!auth.Status || !auth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + auth.Error + " " + auth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(auth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");
        return new BadResponseResult(ModelState);
      }

    }

    [HttpPost("vkLogin")]
    public async Task<IActionResult> VkLogin([FromBody] VkAuthCodeViewModel vkAuth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (vkAuth.State != "123456")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorret");
        return new BadResponseResult(ModelState);
      }
      if (!vkAuth.Status || !vkAuth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + vkAuth.Error + " " + vkAuth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(vkAuth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");
        return new BadResponseResult(ModelState);
      }

      //var appAccessTokenResponse =await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=http://localhost:6210/vk-auth-code.html&code={vkAuth.Code}");
      var appAccessTokenResponse = await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id={_vkAuthSettings.AppId}&client_secret={_vkAuthSettings.AppSecret}&redirect_uri=https://2buttons.ru/vk-auth-code.html&code={vkAuth.Code}");
      var appAccessToken = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);


      var user = await GetOrCreateUserAccountFromVk(appAccessToken.ExternalId, appAccessToken.AccessToken,
        appAccessToken.ExternalEmail);
      if (user == null)
        return new BadResponseResult("We can not find or create you.");

      var accessToken = await AccessToken(user);


    }



    private async Task<UserDto> GetOrCreateUserAccountFromVk(int externalUserId, string externalToken, string email)
    {
      var user = await _db.Users.GetUserByExternalUserIdAsync(externalUserId, SocialType.Facebook);
      if (user != null)
        return user;

      user = await _db.Users.GetUserByEmail(email);
      if (user != null)
      {
        user.FacebookId = externalUserId;
        user.FacebookToken = externalToken;
        await _db.Users.AddOrChangeExternalUserIdAsync(user.UserId, user.FacebookId, SocialType.Facebook,
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
      await _db.Users.AddUserAsync(userDb);

      var bdate = Convert.ToDateTime(userInfo.Birthday);

      var links = await UploadAvatars(userDb.UserId, userInfo.SmallPhoto, userInfo.FullPhoto);
      var userMain = new UserInfoDb
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

   


    [HttpPost("fbLogin")]
    public async Task<IActionResult> FbLogin([FromBody] FacebookAuthViewModel fbAuth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (fbAuth.State != "123456")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorret");
        return new BadResponseResult(ModelState);
      }
      if (!fbAuth.Status || !fbAuth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + fbAuth.Error + " " + fbAuth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(fbAuth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");


        // 1.generate an app access token
        var appAccessTokenResponse =
          await Client.GetStringAsync(
            $"https://graph.facebook.com/v3.0/oauth/access_token?client_id={_fbAuthSettings.AppId}&redirect_uri=https://2buttons.ru/facebook-auth-code.html&client_secret={_fbAuthSettings.AppSecret}&code={fbAuth.Code}");
        var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
        if (!appAccessToken.Error.IsNullOrEmpty())
          return new BadResponseResult(fbAuth.Error + " " + fbAuth.ErrorDescription);

        var user = await GetOrCreateUserAccountFromFb(appAccessToken.AccessToken);
        if (user == null) return new BadResponseResult("We can not find or create account.");

        return await AccessToken(user);
      }
    }


    private async Task<UserDto> GetOrCreateUserAccountFromFb(string externalToken)
      {
        var userInfoResponse = await Client.GetStringAsync(
          $"https://graph.facebook.com/v3.0/me?fields=id,email,first_name,last_name,name,gender,locale,hometown,birthday,picture&access_token={externalToken}");
        var userInfo = JsonConvert.DeserializeObject<FacebookUserResponse>(userInfoResponse);


        var user = await _db.Users.GetUserByExternalUserIdAsync(userInfo.ExternalId, SocialType.Facebook);
        if (user != null)
          return user;
        user = await _db.Users.GetUserByEmail(userInfo.Email);
        if (user != null)
        {
          user.FacebookId = userInfo.ExternalId;
          user.FacebookToken = externalToken;
          await _db.Users.AddOrChangeExternalUserIdAsync(user.UserId, user.FacebookId, SocialType.Facebook,
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
          FacebookId = userInfo.ExternalId,
          FacebookToken = externalToken,
          RegistrationDate = DateTime.UtcNow
        };
        await _db.Users.AddUserAsync(userDb);

        var bdate = Convert.ToDateTime(userInfo.Birthday);

        var smallUrlResponse = await Client.GetStringAsync(
          $"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=normal");
        var smallUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(smallUrlResponse).Response.Url;

        var largeUrlResponse = await Client.GetStringAsync(
          $"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=large");
        var largeUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(largeUrlResponse).Response.Url;
        var links = await UploadAvatars(userDb.UserId, smallUrl, largeUrl);

        await _db.Users.AddUserIntoMainDbAsync(userDb.UserId, userInfo.FirstName + " " + userInfo.LastName, bdate,
          userInfo.SexType, userInfo.City?.Title?.Split(',').FirstOrDefault(), "", links.Item1, links.Item2);


        return userDb.ToUserDto();
      } 
    }
  }