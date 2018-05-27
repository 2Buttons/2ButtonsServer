using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Newtonsoft.Json;

namespace AuthorizationServer.Infrastructure.Services
{
  public class ExternalAuthService : IExternalAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IFbService _fbService;
    private readonly IVkService _vkService;

    public ExternalAuthService(AuthorizationUnitOfWork db, IVkService vkService, IFbService fbService)
    {
      _db = db;
      _vkService = vkService;
      _fbService = fbService;
    }

    public void Dispose()
    {
      _db.Dispose();
    }

    public async Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType)
    {
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
        case SocialType.Facebook:
          socialUserData = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          socialUserData = await _vkService.GetUserInfoAsync(code);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support loging in via {socialType}.");
      }

      var user = await _db.Socials.FindUserByExternalUserIdAsync(socialUserData.ExternalId, socialType);
      if (user != null) return user;
      user = await FindUserByExternalEmail(socialUserData.ExternalEmail);
      if (user != null)
      {
        if (await AddUserSocialAsync(user.UserId, socialType, socialUserData)) return user;
        throw new Exception(
          $"We can not your information about your {socialType} account in our system. Please, log in via email or phone or change social network to another.");
      }
      return await RegisterViaExternalSocial(socialUserData);
    }

    private async Task<UserDto> RegisterViaExternalSocial(NormalizedSocialUserData user)
    {
      const RoleType role = RoleType.User;

      var userDb = new UserDb {Email = user.ExternalEmail, RoleType = role, RegistrationDate = DateTime.UtcNow};
      var isAdded = await _db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      var avatarsLink = await UploadAvatars(userDb.UserId, user.SmallPhotoUrl, user.FullPhotoUrl);

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        Sex = user.Sex,
        City = user.City,
        FullAvatarLink = avatarsLink.smallLink,
        SmallAvatarLink = avatarsLink.fullLink
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your indformation. Please, tell us about it.");
      }

      return new UserDto {UserId = userDb.UserId, RoleType = userDb.RoleType};
    }

    public async Task<bool> AddUserSocialAsync(int internalId, SocialType socialType,
      NormalizedSocialUserData socialUserData)
    {
      var social = new SocialDb
      {
        InternalId = internalId,
        SocialType = socialType,
        ExternalId = socialUserData.ExternalId,
        Email = socialUserData.ExternalEmail,
        ExternalToken = socialUserData.ExternalToken,
        ExpiresIn = socialUserData.ExpiresIn
      };
      return await _db.Socials.AddUserSocialAsync(social);
    }

    private async Task<UserDto> FindUserByExternalEmail(string externalEmail)
    {
      if (externalEmail.IsNullOrEmpty()) return null;
      var user = await _db.Users.GetUserByInternalEmail(externalEmail);
      if (user != null) return user;
      return await _db.Socials.FindUserByExternalEmaildAsync(externalEmail);
    }

    private async Task<(string smallLink, string fullLink)> UploadAvatars(int userId, string smallPhotoUrl,
      string fullPhotoUrl)
    {
      var jsonSmall = JsonConvert.SerializeObject(new {userId, size = 0, url = smallPhotoUrl});
      var jsonFull = JsonConvert.SerializeObject(new {userId, size = 1, url = fullPhotoUrl});
      var s = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonSmall);
      var f = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonFull);

      await Task.WhenAll(f, s);
      return (s.Result, f.Result);
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
  }
}