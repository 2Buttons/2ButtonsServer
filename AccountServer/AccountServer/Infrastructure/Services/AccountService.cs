using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.DTO;
using AccountServer.ViewModels;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;

namespace AccountServer.Infrastructure.Services
{
  public class AccountService
  {
    private readonly AccountDataUnitOfWork _db;

    public AccountService(AccountDataUnitOfWork accountDb)
    {
      _db = accountDb;
    }

    public async Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId)
    {
      var userInfoTask =  _db.Accounts.GetUserInfo(userId, userPageId);
      var userStatisticsTask =  _db.Accounts.GetUserStatistics(userPageId);
      var userContactsTask = _db.Users.GetUserSocialsAsync(userPageId);

      await Task.WhenAll(userInfoTask, userStatisticsTask, userContactsTask);

      var user = userInfoTask.Result.MapToUserInfoViewModel();
      user.UserStatistics = userStatisticsTask.Result.MapToUserStatisticsViewModel();

      if (userId != userPageId) user.UserStatistics.AnsweredQuestions = 0;

      user.Social = ConvertContactsDtoToViewModel(userContactsTask.Result);
      return user;
    }

    private List<UserContactsViewModel> ConvertContactsDtoToViewModel(UserContactsDto userContacts)
    {
      var result = new List<UserContactsViewModel>();

      if (userContacts.VkId != 0)
        result.Add(new UserContactsViewModel
        {
          SocialNetType = SocialNetType.Vk,
          AccountUrl = "https://vk.com/id" + userContacts.VkId
        });
      if (userContacts.FacebookId != 0)
        result.Add(new UserContactsViewModel
        {
          SocialNetType = SocialNetType.Facebook,
          AccountUrl = "https://www.facebook.com/profile.php?id=" + userContacts.FacebookId
        });

      return result;
    }
  }
}