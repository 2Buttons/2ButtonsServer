using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.Entities;
using AccountData.DTO;
using AccountServer.ViewModels;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Helpers;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Http;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly IFbService _fbService;
    private readonly IVkService _vkService;

    public AccountService(AccountDataUnitOfWork accountDb, IVkService vkService, IFbService fbService)
    {
      _db = accountDb;
      _vkService = vkService;
      _fbService = fbService;
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      var userInfoTask = _db.UsersInfo.GetUserInfoAsync(userId, userPageId);
      var userStatisticsTask = _db.UsersInfo.GetUserStatisticsAsync(userPageId);
      var userContactsTask = _db.Users.GetUserSocialsAsync(userPageId);

      await Task.WhenAll(userInfoTask, userStatisticsTask, userContactsTask);

      var user = userInfoTask.Result.MapToUserInfoViewModel();
      user.UserStatistics = userStatisticsTask.Result.MapToUserStatisticsViewModel();

      if (userId != userPageId) user.UserStatistics.AnsweredQuestions = 0;

      user.Social = ConvertContactsDtoToViewModel(userContactsTask.Result);
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      return await _db.UsersInfo.UpdateUserInfoAsync(user);
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

      var userInfo = await _db.UsersInfo.GetUserInfoAsync(userId, userId);

      if (userInfo.SmallAvatarLink.IsNullOrEmpty() || userInfo.SmallAvatarLink.Contains("stan") && !user.SmallPhotoUrl.IsNullOrEmpty())
      {
        userInfo.SmallAvatarLink = await MediaServerHelper.UploadAvatarUrl(AvatarSizeType.SmallAvatar, user.SmallPhotoUrl) ?? MediaServerHelper.StandardAvatar(AvatarSizeType.SmallAvatar);
      }
      if (userInfo.FullAvatarLink.IsNullOrEmpty() || userInfo.FullAvatarLink.Contains("stan") && !user.LargePhotoUrl.IsNullOrEmpty())
      {
        userInfo.FullAvatarLink = await MediaServerHelper.UploadAvatarUrl(AvatarSizeType.LargeAvatar, user.LargePhotoUrl) ?? MediaServerHelper.StandardAvatar(AvatarSizeType.LargeAvatar);
      }

      UpdateUserInfoDto updateUser = new UpdateUserInfoDto
      {
        UserId = userInfo.UserId,
        Login = userInfo.Login,
        BirthDate = userInfo.BirthDate,
        Sex = userInfo.Sex,
        City = userInfo.City,
        Description = userInfo.Description,
        LargeAvatarLink = userInfo.FullAvatarLink,
        SmallAvatarLink = userInfo.SmallAvatarLink,
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

    public async Task<bool> UpdateAvatarViaLink(int userId, AvatarSizeType avatarSize, string newAvatarUrl)
    {
      var url = await MediaServerHelper.UploadAvatarUrl(avatarSize, newAvatarUrl);
      if (url.IsNullOrEmpty()) return false;
      return await _db.UsersInfo.UpdateUserLargeAvatar(userId, url);
    }

    public async Task<bool> UpdateAvatarViaFile(int userId, AvatarSizeType avatarSize, IFormFile file)
    {
      var url = await MediaServerHelper.UploadAvatarFile(avatarSize, file);
      if (url.IsNullOrEmpty()) return false;
      return await _db.UsersInfo.UpdateUserLargeAvatar(userId, url);
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