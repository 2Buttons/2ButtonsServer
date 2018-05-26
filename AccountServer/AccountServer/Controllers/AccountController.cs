using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.DTO;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("/account")]
  public class AccountController : Controller
  {
    private readonly AccountDataUnitOfWork _db;

    public AccountController(AccountDataUnitOfWork accountDb)
    {
      _db = accountDb;
    }

    [Authorize]
    [HttpGet("user/{userPageId:int}")]
    public async Task<IActionResult> GetUserInfoAuth(int userPageId)
    {
      if (userPageId == 0)
        return new BadResponseResult("The wrong route.");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      var userInfo = await _db.Accounts.GetUserInfo(userId, userPageId);
      var userStatistics = await _db.Accounts.GetUserStatistics(userPageId);
      var userContacts = _db.Users.GetUserSocialsAsync(userPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      if (userId != userPageId)
        result.UserStatistics.AnsweredQuestions = 0;

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return new OkResponseResult(result);
    }

    [Authorize]
    [HttpPost("getUserInfoAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return new BadResponseResult("Input body is null.");
      if (!ModelState.IsValid)
        return new BadResponseResult("Validation error.", ModelState);

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      var userInfo = await _db.Accounts.GetUserInfo(userId, userPage.UserPageId);
      var userStatistics = await _db.Accounts.GetUserStatistics(userPage.UserPageId);
      var userContacts = _db.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      if (userPage.UserId != userPage.UserPageId)
        result.UserStatistics.AnsweredQuestions = 0;

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return new OkResponseResult(result);
    }

    [HttpPost("getUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return new BadResponseResult($"Input body  is null");
      if (!ModelState.IsValid)
        return new BadResponseResult("Validation error.", ModelState);

      var userInfo = await _db.Accounts.GetUserInfo(userPage.UserId, userPage.UserPageId);
      var userStatistics = await _db.Accounts.GetUserStatistics(userPage.UserPageId);
      var userContacts = _db.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      if (userPage.UserId != userPage.UserPageId)
        result.UserStatistics.AnsweredQuestions = 0;

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return new OkResponseResult(result);
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