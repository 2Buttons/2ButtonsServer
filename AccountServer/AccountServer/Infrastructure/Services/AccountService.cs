using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.Entities;
using AccountData.DTO;
using AccountData.Main.Entities;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly IVkService _vkService;
    private readonly IFbService _fbService;

    public AccountService(AccountDataUnitOfWork accountDb, IVkService vkService, IFbService fbService)
    {
      _db = accountDb;
      _vkService = vkService;
      _fbService = fbService;
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      var userInfoTask = _db.Accounts.GetUserInfoAsync(userId, userPageId);
      var userStatisticsTask = _db.Accounts.GetUserStatisticsAsync(userPageId);
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
      NormalizedSocialUserData user;
      switch (socialType)
      {

        case SocialType.Facebook:
          user = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          user = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default:
          throw new System.Exception("We do not support this social network.");
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
      return await _db.Users.AddUserSocialAsync(social);
    }
    private List<UserContactsViewModel> ConvertContactsDtoToViewModel(List<UserSocialDto> userContacts)
    {
      var result = new List<UserContactsViewModel>();
      foreach (var user in userContacts)
      {
        switch (user.SocialType)
        {
          case SocialType.Nothing:
            break;
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
          case SocialType.Twiter:
            break;
          case SocialType.GooglePlus:
            break;
          case SocialType.Telegram:
            break;
          case SocialType.Badoo:
            break;
        }
      }
      return result;
    }
  }
}