using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.Entities;
using AccountData.DTO;
using AccountServer.ViewModels;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Http;
using System.Linq;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.MediaFolders;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly IFbService _fbService;
    private readonly IVkService _vkService;
    private readonly ConnectionsHub _hub;

    public AccountService(AccountDataUnitOfWork accountDb, ConnectionsHub hub, IVkService vkService, IFbService fbService)
    {
      _db = accountDb;
      _hub = hub;
      _vkService = vkService;
      _fbService = fbService;
    }


    public async Task<string> GetUserAvatar(int userId, AvatarSizeType avatarSizeType)
    {
      var avatar = await _db.UsersInfo.GetUserAvatar(userId);
      return MediaConverter.ToFullAvatarUrl(avatar, avatarSizeType);
    }

    public async Task<(string city, DateTime birthdate)> GetCityAndBirthdate(int userId)
    {
      var userInfoTask = await _db.UsersInfo.FindUserInfoAsync(userId, userId);
      return (userInfoTask.City, userInfoTask.BirthDate);
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      var userInfoTask = _db.UsersInfo.FindUserInfoAsync(userId, userPageId);
      var userStatisticsTask = _db.UsersInfo.GetUserStatisticsAsync(userPageId);
      var userContactsTask = _db.Users.GetUserSocialsAsync(userPageId);

      await Task.WhenAll(userInfoTask, userStatisticsTask, userContactsTask);

      var user = userInfoTask.Result.MapToUserInfoViewModel();
      user.UserStatistics = userStatisticsTask.Result.MapToUserStatisticsViewModel();

      if (userId != userPageId) user.UserStatistics.AnsweredQuestions = 0;

      user.Social = ConvertContactsDtoToViewModel(userContactsTask.Result);
      await _hub.Monitoring.UpdateUrlMonitoring(userId,
        userId != userPageId ? UrlMonitoringType.OpensUserPage : UrlMonitoringType.OpensPersonalPage);
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      var oldUser = await _db.UsersInfo.FindUserInfoAsync(user.UserId, user.UserId);
      if (oldUser == null) throw new NotFoundException("This user does not exist");

      var updateUser = new UpdateUserInfoDto()
      {
        UserId = user.UserId,
        Login  = user.Login.IsNullOrEmpty() ? oldUser.Login : user.Login,
        BirthDate = user.BirthDate.Year<1900 ? oldUser.BirthDate : user.BirthDate,
        SexType = user.SexType == SexType.Both ? oldUser.SexType : user.SexType,
        City = user.Login.IsNullOrEmpty() ? oldUser.City : user.City,
         OriginalAvatarUrl = user.OriginalAvatarUrl,
        Description = user.Description.IsNullOrEmpty() ? oldUser.Description : user.Description
      };

      return await _db.UsersInfo.UpdateUserInfoAsync(updateUser);
    }

    public async Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType)
    {
      NormalizedSocialUserData user;
      switch (socialType)
      {
        case SocialType.Facebook:
          user = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          user = await _vkService.GetUserInfoAsync(code);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception("We do not support this social network.");
      }
      var social = new SocialDb
      {
        InternalId = userId,
        ExternalId = user.ExternalId,
        SocialType = socialType,
        Email = user.ExternalEmail,
        ExternalToken = user.ExternalToken,
        ExpiresIn = user.ExpiresIn
      };

      if (!await _db.Users.AddUserSocialAsync(social)) return false;

      var userInfo = await _db.UsersInfo.FindUserInfoAsync(userId, userId);

      if (userInfo.OriginalAvatarUrl.IsNullOrEmpty() || MediaConverter.IsStandardBackground(userInfo.OriginalAvatarUrl) && !user.OriginalPhotoUrl.IsNullOrEmpty())
      {
        userInfo.OriginalAvatarUrl = await UploadAvatarUrlOrGetStandard(user.OriginalPhotoUrl);
      }


     

      UpdateUserInfoDto updateUser = new UpdateUserInfoDto
      {
        UserId = userInfo.UserId,
        Login = userInfo.Login,
        BirthDate = userInfo.BirthDate,
        SexType = userInfo.SexType,
        City = userInfo.City,
        Description = userInfo.Description,
      };

      try
      {
        await _db.UsersInfo.UpdateUserInfoAsync(updateUser);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return true;
    }


    private async Task<string> UploadAvatarUrlOrGetStandard(string avatarUrl)
    {
      return await _hub.Media.UploadAvatarUrl(AvatarType.Custom, avatarUrl) ?? (await _hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault();
    }

    public async Task<(bool isUpdated, string url)> UpdateAvatarViaUrl(int userId, AvatarType avatarType, string newAvatarUrl)
    {
      var url = await _hub.Media.UploadAvatarUrl( AvatarType.Custom, newAvatarUrl);
      if (url.IsNullOrEmpty()) return (false, null);
      return (await _db.UsersInfo.UpdateUserOriginalAvatar(userId, url), url);
    }

    public async Task<(bool isUpdated, string url)> UpdateAvatarViaFile(int userId, AvatarType avatarType,
      IFormFile file)
    {
      var url = await _hub.Media.UploadAvatarFile(AvatarType.Custom, file);
      if (url.IsNullOrEmpty()) return (false, null);
      return (await _db.UsersInfo.UpdateUserOriginalAvatar(userId, url), url);
    }

    public void Dispose()
    {
      _db.Dispose();
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