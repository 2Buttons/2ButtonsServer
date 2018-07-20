using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
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
    private readonly ConnectionsHub _hub;

    public ExternalAuthService(AuthorizationUnitOfWork db, ConnectionsHub hub, IVkService vkService, IFbService fbService)
    {
      _db = db;
      _hub = hub;
      _vkService = vkService;
      _fbService = fbService;
    }

    public void Dispose()
    {
      _db.Dispose();
    }


    public async Task<UserDto> GetUserViaExternalSocialNet(long externalUserId, string email, string externalToken, long expiresIn, SocialType socialType)
    {
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
        
        case SocialType.Vk:
          socialUserData = await _vkService.GetUserInfoAsync(externalUserId,  email,  externalToken,  expiresIn);
          break;
        case SocialType.Facebook:
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support mobile logging via {socialType}.");
      }

      return await ExternalUserProcessing(socialUserData, socialType);

    }


    public async Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType, bool isTest = false)
    {
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
        case SocialType.Facebook:
          socialUserData = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          socialUserData = await _vkService.GetUserInfoAsync(code, isTest);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support logging via {socialType}.");
      }
      return await ExternalUserProcessing(socialUserData, socialType);
    }

    private async Task<UserDto> ExternalUserProcessing(NormalizedSocialUserData socialUserData, SocialType socialType)
    { 

      var user = await _db.Socials.FindUserByExternalUserIdAsync(socialUserData.ExternalId, socialType);
      if (user != null)
      {
        if (socialUserData.ExpiresIn == 0)
          await _db.Socials.UpdateExternalAccessToken(socialUserData.ExternalId, socialType, socialUserData.ExternalToken,
            socialUserData.ExpiresIn);
        return user;
      }

      user = await FindUserByEmail(socialUserData.ExternalEmail);
      if (user == null) return await RegisterViaExternalSocial(socialUserData, socialType);

      if(!await AddExternalEnter(user.UserId, socialType, socialUserData))
        throw new Exception(
          $"We can not your information about your {socialType} account in our system. Please, log in via email or phone or change social network to another.");
      return user;
    }

    private async Task<bool> AddExternalEnter(int userId, SocialType socialType, NormalizedSocialUserData socialUserData)
    {
      if (!await AddUserSocialAsync(userId, socialType, socialUserData)) return false;
      var userInfo = await _db.UsersInfo.GetUserInfoAsync(userId);
      if (userInfo.LargeAvatarLink.IsNullOrEmpty() ||  MediaConverter.IsStandardBackground(userInfo.LargeAvatarLink) && !socialUserData.OriginalPhotoUrl.IsNullOrEmpty())
      {
        userInfo.LargeAvatarLink = await _hub.Media.UploadAvatarUrl(AvatarType.Custom, socialUserData.OriginalPhotoUrl) ?? (await _hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault();
      }

      await _db.UsersInfo.UpdateUserInfoAsync(userInfo);
      return true;
    }

    private async Task<UserDto> RegisterViaExternalSocial(NormalizedSocialUserData user, SocialType socialType)
    {
      const RoleType role = RoleType.User;

      var userDb = new UserDb { Email = user.ExternalEmail, RoleType = role, RegistrationDate = DateTime.UtcNow };
      var isAdded = await _db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      var fullLink = _hub.Media.StandardAvatar(AvatarSizeType.Large);
      if (!user.LargePhotoUrl.IsNullOrEmpty())
      {
        var url = await _hub.Media.UploadAvatarUrl(AvatarSizeType.Large, user.LargePhotoUrl);
        if (!url.IsNullOrEmpty())
          fullLink = url;
      }

      var smallLink = _hub.Media.StandardAvatar(AvatarSizeType.Small);
      if (!user.SmallPhotoUrl.IsNullOrEmpty())
      {
        var url = await _hub.Media.UploadAvatarUrl(AvatarSizeType.Small, user.SmallPhotoUrl);
        if (!url.IsNullOrEmpty())
          smallLink = url;
      }
     // = !user.LargePhotoUrl.IsNullOrEmpty() ? (await MediaServerHelper.UploadAvatarUrl(AvatarSizeType.LargeAvatar, user.LargePhotoUrl)).IsNullOrEmpty() :  MediaServerHelper.StandardAvatar(AvatarSizeType.LargeAvatar);
     // var smallLink = !user.SmallPhotoUrl.IsNullOrEmpty() ? await MediaServerHelper.UploadAvatarUrl(AvatarSizeType.SmallAvatar, user.SmallPhotoUrl) :  MediaServerHelper.StandardAvatar(AvatarSizeType.SmallAvatar);

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        SexType = user.SexType,
        City = user.City,
        LargeAvatarLink = fullLink,
        SmallAvatarLink = smallLink
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your information. Please, tell us about it.");
      }

     
      if (!await AddUserSocialAsync(userDb.UserId, socialType, user))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your social information. Please, tell us about it.");
      }

      await _hub.Monitoring.AddUrlMonitoring(userDb.UserId);

      return new UserDto { UserId = userDb.UserId, RoleType = userDb.RoleType };
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

    private async Task<UserDto> FindUserByEmail(string email)
    {
      if (email.IsNullOrEmpty()) return null;
      var user = await _db.Users.GetUserByInternalEmail(email);
      if (user != null) return user;
      return await _db.Socials.FindUserByExternalEmaildAsync(email);
    }
  }
}