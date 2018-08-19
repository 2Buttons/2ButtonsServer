using System;
using System.Linq;
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
using Microsoft.Extensions.Logging;

namespace AuthorizationServer.Infrastructure.Services
{
  public class ExternalAuthService : IExternalAuthService
  {
    private  AuthorizationUnitOfWork Db { get; }
    private  IFbService FbService { get; }
    private  ConnectionsHub Hub { get; }
    private  IVkService VkService { get; }
    private  ILogger<ExternalAuthService> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public ExternalAuthService(AuthorizationUnitOfWork db, ConnectionsHub hub, IVkService vkService,
      IFbService fbService, ILogger<ExternalAuthService> logger, MediaConverter  mediaConverter)
    {
      Db = db;
      Hub = hub;
      VkService = vkService;
      FbService = fbService;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    public void Dispose()
    {
      Db.Dispose();
    }

    public async Task<UserDto> GetUserViaExternalSocialNet(long externalUserId, string email, string externalToken,
      long expiresIn, SocialType socialType)
    {
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(GetUserViaExternalSocialNet)}.Start Via email from mobile");
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
        case SocialType.Vk:
          socialUserData = await VkService.GetUserInfoAsync(externalUserId, email, externalToken, expiresIn);
          break;
        case SocialType.Facebook:
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support mobile logging via {socialType}.");
      }

      var result =  await ExternalUserProcessing(socialUserData, socialType);
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(GetUserViaExternalSocialNet)}.End Via email from mobile");
      return result;
    }

    public async Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType, bool isTest = false)
    {
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(GetUserViaExternalSocialNet)}.Start via code from web client");
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
       
          //socialUserData = await _fbService.GetUserInfoAsync(code);
          //break;
        case SocialType.Vk:
          socialUserData = await VkService.GetUserInfoAsync(code, isTest);
          break;
        case SocialType.Facebook:
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support logging via {socialType}.");
      }
      var result =  await ExternalUserProcessing(socialUserData, socialType);
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(GetUserViaExternalSocialNet)}.End via code from web client");
      return result;
    }

    public async Task<bool> AddUserSocialAsync(int internalId, SocialType socialType,
      NormalizedSocialUserData socialUserData)
    {
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(AddUserSocialAsync)}.Start");
      var social = new SocialDb
      {
        InternalId = internalId,
        SocialType = socialType,
        ExternalId = socialUserData.ExternalId,
        Email = socialUserData.ExternalEmail,
        ExternalToken = socialUserData.ExternalToken,
        ExpiresIn = socialUserData.ExpiresIn
      };
      var result =  await Db.Socials.AddUserSocialAsync(social);
      Logger.LogInformation($"{nameof(ExternalAuthService)}.{nameof(AddUserSocialAsync)}.End");
      return result;
    }

    private async Task<UserDto> ExternalUserProcessing(NormalizedSocialUserData socialUserData, SocialType socialType)
    {
      var user = await Db.Socials.FindUserByExternalUserIdAsync(socialUserData.ExternalId, socialType);
      if (user != null)
      {
        if (socialUserData.ExpiresIn == 0)
          await Db.Socials.UpdateExternalAccessToken(socialUserData.ExternalId, socialType,
            socialUserData.ExternalToken, socialUserData.ExpiresIn);
        return user;
      }

      user = await FindUserByEmail(socialUserData.ExternalEmail);
      if (user == null) return await RegisterViaExternalSocial(socialUserData, socialType);

      if (!await AddExternalEnter(user.UserId, socialType, socialUserData))
        throw new Exception(
          $"We can not your information about your {socialType} account in our system. Please, log in via email or phone or change social network to another.");
      return user;
    }

    private async Task<bool> AddExternalEnter(int userId, SocialType socialType,
      NormalizedSocialUserData socialUserData)
    {
      if (!await AddUserSocialAsync(userId, socialType, socialUserData)) return false;
      var userInfo = await Db.UsersInfo.GetUserInfoAsync(userId);
      if (userInfo.OriginalAvatarUrl.IsNullOrEmpty() ||
          MediaConverter.IsStandardBackground(userInfo.OriginalAvatarUrl) &&
          !socialUserData.OriginalPhotoUrl.IsNullOrEmpty())
        userInfo.OriginalAvatarUrl = await UploadAvatarUrlOrGetStandard(socialUserData.OriginalPhotoUrl);

      await Db.UsersInfo.UpdateUserInfoAsync(userInfo);
      return true;
    }

    private async Task<string> UploadAvatarUrlOrGetStandard(string avatarUrl)
    {
      return await Hub.Media.UploadAvatarUrl(AvatarType.Custom, avatarUrl) ??
             (await Hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault();
    }

    private async Task<UserDto> RegisterViaExternalSocial(NormalizedSocialUserData user, SocialType socialType)
    {
      const RoleType role = RoleType.User;

      var userDb = new UserDb {Email = user.ExternalEmail, RoleType = role, RegistrationDate = DateTime.UtcNow};
      var isAdded = await Db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      var fullUrl = await UploadAvatarUrlOrGetStandard(user.OriginalPhotoUrl);

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        SexType = user.SexType,
        City = user.City,
        OriginalAvatarUrl = fullUrl,
        Description = ""
      };

      if (!await Db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await Db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your information. Please, tell us about it.");
      }

      if (!await AddUserSocialAsync(userDb.UserId, socialType, user))
      {
        await Db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your social information. Please, tell us about it.");
      }

      await Hub.Monitoring.AddUrlMonitoring(userDb.UserId);

      return new UserDto {UserId = userDb.UserId, RoleType = userDb.RoleType};
    }

    private async Task<UserDto> FindUserByEmail(string email)
    {
      if (email.IsNullOrEmpty()) return null;
      var user = await Db.Users.GetUserByInternalEmail(email);
      if (user != null) return user;
      return await Db.Socials.FindUserByExternalEmaildAsync(email);
    }
  }
}