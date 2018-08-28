using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountData;
using AccountData.DTO;
using AccountServer.ViewModels;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Entities.Acccount;
using CommonLibraries.Entities.Main;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private AccountDataUnitOfWork Db { get; }
    private IFbService FbService { get; }
    private IVkService VkService { get; }
    private ConnectionsHub Hub { get; }
    private ILogger<AccountService> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public AccountService(AccountDataUnitOfWork accountDb, ConnectionsHub hub, IVkService vkService,
      IFbService fbService, ILogger<AccountService> logger, MediaConverter mediaConverter)
    {
      Db = accountDb;
      Hub = hub;
      VkService = vkService;
      FbService = fbService;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    public async Task<string> GetUserAvatar(int userId, AvatarSizeType avatarSizeType)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetUserAvatar)}.Start");
      var avatar = await Db.UsersInfo.GetUserAvatar(userId);
      var result = MediaConverter.ToFullAvatarUrl(avatar, avatarSizeType);
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetUserAvatar)}.End");
      return result;
    }

    public async Task<(string city, DateTime? birthdate)> GetCityAndBirthdate(int userId)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetCityAndBirthdate)}.Start");
      var userInfoTask = await Db.UsersInfo.FindUserInfoAsync(userId, userId);
      var result = (userInfoTask.City, userInfoTask.BirthDate.Date != DateTime.MinValue.Date
        ? userInfoTask.BirthDate
        : new DateTime?());
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetCityAndBirthdate)}.End");
      return result;
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetUserAsync)}.Start");
      var userInfoTask = Db.UsersInfo.FindUserInfoAsync(userId, userPageId);
      var userStatisticsTask = Db.UsersInfo.GetUserStatisticsAsync(userPageId);
      var userContactsTask = Db.Users.GetUserSocialsAsync(userPageId);

      await Task.WhenAll(userInfoTask, userStatisticsTask, userContactsTask);

      var user = userInfoTask.Result.MapToUserInfoViewModel(MediaConverter);
      user.UserStatistics = userStatisticsTask.Result.MapToUserStatisticsViewModel();

      if (userId != userPageId) user.UserStatistics.AnsweredQuestions = 0;

      user.Social = ConvertContactsDtoToViewModel(userContactsTask.Result);
      await Hub.Monitoring.UpdateUrlMonitoring(userId,
        userId != userPageId ? UrlMonitoringType.OpensUserPage : UrlMonitoringType.OpensPersonalPage);
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(GetUserAsync)}.End");
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateUserInfoAsync)}.Start");
      var oldUser = await Db.UsersInfo.FindUserInfo(user.UserId);
      if (oldUser == null) throw new NotFoundException("This user does not exist");

      var updateUser = new UserInfoEntity
      {
        UserId = user.UserId,
        FirstName = user.FristName.IsNullOrEmpty() ? oldUser.FirstName : user.FristName,
        LastName = user.LastName.IsNullOrEmpty() ? oldUser.LastName : user.LastName,
        BirthDate = user.BirthDate.Year < 1900 ? oldUser.BirthDate : user.BirthDate,
        SexType = user.SexType == SexType.Both ? oldUser.SexType : user.SexType,
        CityId = user.City.IsNullOrEmpty() ? oldUser.CityId : (await Db.Cities.GetOrCreateCity(user.City)).CityId,
        OriginalAvatarUrl = oldUser.OriginalAvatarUrl,
        Description = user.Description.IsNullOrEmpty() ? oldUser.Description : user.Description
      };

      var result = await Db.UsersInfo.UpdateUserInfoAsync(updateUser);
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateUserInfoAsync)}.End");
      return result;
    }

    public async Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(AddUserSocialAsync)}.Start");
      NormalizedSocialUserData user;
      switch (socialType)
      {
        case SocialType.Facebook:
          user = await FbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          user = await VkService.GetUserInfoAsync(code);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception("We do not support this social network.");
      }
      var social = new SocialEntity
      {
        InternalId = userId,
        ExternalId = user.ExternalId,
        SocialType = socialType,
        Email = user.ExternalEmail,
        ExternalToken = user.ExternalToken,
        ExpiresIn = user.ExpiresIn
      };

      if (!await Db.Users.AddUserSocialAsync(social)) return false;

      var userInfo = await Db.UsersInfo.FindUserInfoAsync(userId, userId);

      if (userInfo.OriginalAvatarUrl.IsNullOrEmpty() ||
          MediaConverter.IsStandardBackground(userInfo.OriginalAvatarUrl) && !user.OriginalPhotoUrl.IsNullOrEmpty())
        userInfo.OriginalAvatarUrl = await UploadAvatarUrlOrGetStandard(user.OriginalPhotoUrl);

      var updateUser = new UpdateUserInfoDto
      {
        UserId = userInfo.UserId,
        FristName = userInfo.FirstName,
        LastName = userInfo.LastName,
        BirthDate = userInfo.BirthDate,
        SexType = userInfo.SexType,
        City = userInfo.City,
        Description = userInfo.Description
      };

      try
      {
        await UpdateUserInfoAsync(updateUser);
      }
      catch (Exception e)
      {
        Logger.LogError(e, $"{nameof(AccountService)}.{nameof(AddUserSocialAsync)} Error");
      }
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(AddUserSocialAsync)}.End");
      return true;
    }

    public async Task<(bool isUpdated, string url)> UpdateAvatarViaUrl(int userId, AvatarType avatarType,
      AvatarSizeType avatarSizeType, string newAvatarUrl)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateAvatarViaUrl)}.Start ");
      var url = await Hub.Media.UploadAvatarUrl(AvatarType.Custom, newAvatarUrl);
      var result = url.IsNullOrEmpty()
        ? (false, null)
        : (await Db.UsersInfo.UpdateUserOriginalAvatar(userId, url), MediaConverter.ToFullAvatarUrl(url, avatarSizeType)
        );
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateAvatarViaUrl)}.End ");
      return result;
    }

    public async Task<(bool isUpdated, string url)> UpdateAvatarViaFile(int userId, AvatarType avatarType,
      AvatarSizeType avatarSizeType, IFormFile file)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateAvatarViaFile)}.Start ");
      var url = await Hub.Media.UploadAvatarFile(AvatarType.Custom, file);
      var result = url.IsNullOrEmpty()
        ? (false, null)
        : (await Db.UsersInfo.UpdateUserOriginalAvatar(userId, url), MediaConverter.ToFullAvatarUrl(url, avatarSizeType)
        );
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UpdateAvatarViaFile)}.End ");
      return result;
    }

    public (string FirstName, string LastName) ParseLoginIntoFirstNameAndLastName(string login)
    {
      var result = (firstName: "", lastName: "");
      if (login.IsNullOrEmpty()) return result;
      var spacePosition = login.IndexOf(' ');
      if (spacePosition == -1) spacePosition = login.Length - 1;
      var firstName = login.Substring(0, spacePosition);
      var lastName = login.Substring(spacePosition + 1);
      result.firstName = firstName;
      result.lastName = lastName;
      return result;
    }

    private async Task<string> UploadAvatarUrlOrGetStandard(string avatarUrl)
    {
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UploadAvatarUrlOrGetStandard)}.Start ");
      var result = await Hub.Media.UploadAvatarUrl(AvatarType.Custom, avatarUrl) ??
                   (await Hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault();
      Logger.LogInformation($"{nameof(AccountService)}.{nameof(UploadAvatarUrlOrGetStandard)}.End ");
      return result;
    }

    public void Dispose()
    {
      Db.Dispose();
    }

    private List<UserContactsViewModel> ConvertContactsDtoToViewModel(List<UserSocialDto> userContacts)
    {
      var result = new List<UserContactsViewModel>();
      foreach (var user in userContacts)
        switch (user.SocialType)
        {
          case SocialType.Nothing: break;
          case SocialType.Facebook:
            result.Add(new UserContactsViewModel
            {
              SocialType = SocialType.Facebook,
              AccountUrl = "https://www.facebook.com/profile.php?id=" + user.ExternalId
            });
            break;
          case SocialType.Vk:
            result.Add(new UserContactsViewModel
            {
              SocialType = SocialType.Vk,
              AccountUrl = "https://vk.com/id" + user.ExternalId
            });
            break;
          case SocialType.Twiter: break;
          case SocialType.GooglePlus: break;
          case SocialType.Telegram: break;
          case SocialType.Badoo: break;
        }
      return result;
    }
  }
}