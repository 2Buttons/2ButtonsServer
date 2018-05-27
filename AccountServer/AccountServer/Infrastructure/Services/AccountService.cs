using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.DTO;
using AccountData.Account.Entities;
using AccountData.DTO;
using AccountData.Main.Entities;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.SocialNetworks.Vk;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly IVkService _vkService;

    public AccountService(AccountDataUnitOfWork accountDb, IVkService vkService)
    {
      _db = accountDb;
      _vkService = vkService;
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      var userInfoTask =  _db.Accounts.GetUserInfoAsync(userId, userPageId);
      var userStatisticsTask =  _db.Accounts.GetUserStatisticsAsync(userPageId);
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
      return await _db.Accounts.UpdateUserInfoAsync(user);
    }

    public async Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType)
    {
      switch (socialType)
      {

        case SocialType.Facebook:
        case SocialType.Vk:
          return await AddVkAsync(userId, code);
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default:
          throw new System.Exception("We do not support this social network.");
      }
    }

    public async Task<bool> AddVkAsync(int userId, string code)
    {
      var accessToken = await _vkService.GetAccessTokenAsync(code);
      var userData = await _vkService.GetUserInfoAsync(accessToken.UserId, accessToken.AccessToken, accessToken.Email);
      var social = new SocialDb
      {
        InternalId = userId,
        ExternalId = userData.ExternalId,
        SocialType = SocialType.Vk,
        Email = userData.ExternalEmail,
        ExternalToken = accessToken.AccessToken,
        ExpiresIn = accessToken.ExpiresIn
      };
      return await _db.Users.AddUserSocialAsync(social);
    }

    public async Task<bool> AddFbAsync(int userId, string code)
    {
      var accessToken = await _vkService.GetAccessTokenAsync(code);
      var userData = await _vkService.GetUserInfoAsync(accessToken.UserId, accessToken.AccessToken, accessToken.Email);
      var social = new SocialDb
      {
        InternalId = userId,
        ExternalId = userData.ExternalId,
        SocialType = SocialType.Vk,
        Email = userData.ExternalEmail,
        ExternalToken = accessToken.AccessToken,
        ExpiresIn = accessToken.ExpiresIn
      };
      return await _db.Users.AddUserSocialAsync(social);
    }

    private List<UserContactsViewModel> ConvertContactsDtoToViewModel(UserContactsDto userContacts)
    {
      var result = new List<UserContactsViewModel>();

      if (userContacts.VkId != 0)
        result.Add(new UserContactsViewModel
        {
          SocialType = SocialType.Vk,
          AccountUrl = "https://vk.com/id" + userContacts.VkId
        });
      if (userContacts.FacebookId != 0)
        result.Add(new UserContactsViewModel
        {
          SocialType = SocialType.Facebook,
          AccountUrl = "https://www.facebook.com/profile.php?id=" + userContacts.FacebookId
        });

      return result;
    }
  }
}