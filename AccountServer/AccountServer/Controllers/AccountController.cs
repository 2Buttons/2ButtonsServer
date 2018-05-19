using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsDatabase;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("/account")]
  public class AccountController : Controller
  {
    private readonly AccountUnitOfWork _accountDb;
    private readonly TwoButtonsUnitOfWork _mainDb;

    public AccountController(TwoButtonsUnitOfWork mainDb, AccountUnitOfWork accountDb)
    {
      _mainDb = mainDb;
      _accountDb = accountDb;
    }

    [Authorize]
    [HttpPost("getUserInfoAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      var userInfo = await _mainDb.Accounts.GetUserInfo(userId, userPage.UserPageId);
       // return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      var userStatistics = await _mainDb.Accounts.GetUserStatistics(userPage.UserPageId);
       // return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      var userContacts = _accountDb.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      if (userPage.UserId != userPage.UserPageId)
        result.UserStatistics.AnsweredQuestions = 0;

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return Ok(result);
    }

    [HttpPost("getUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      var userInfo = await _mainDb.Accounts.GetUserInfo(userPage.UserId, userPage.UserPageId);
      // return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      var userStatistics = await _mainDb.Accounts.GetUserStatistics(userPage.UserPageId);
      // return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      var userContacts = _accountDb.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      if (userPage.UserId != userPage.UserPageId)
        result.UserStatistics.AnsweredQuestions = 0;

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return Ok(result);
    }

    [HttpPost("isUserIdValid")]
    public async Task<IActionResult> IsUserIdValid([FromBody] UserIdValidationViewModel user)
    {
      var isValid = await _accountDb.Users.IsUserIdExistAsync(user.UserId);
      return Ok(new {IsValid = !isValid});
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