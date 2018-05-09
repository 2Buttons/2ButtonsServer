using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Models;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.Models;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;
using RoleType = AccountServer.Helpers.RoleType;
using SocialNetType = AccountServer.Helpers.SocialNetType;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  // [Route("/account")]
  public class AccountController : Controller
  {
    private readonly TwoButtonsContext _twoButtonsContext;
    private readonly AccountUnitOfWork _accountDb;

    public AccountController(TwoButtonsContext context, AccountUnitOfWork accountDb)
    {
      _twoButtonsContext = context;
      _accountDb = accountDb;
    }

    [HttpGet("register")]
    public IActionResult AddUser()
    {
      return BadRequest("Please, use POST request.");
    }

    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] UserRegistrationViewModel user)
    {
      if (user == null)
        return BadRequest($"Input parameter  is null");
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      const RoleType role = RoleType.User;

      if (AccountWrapper.TryAddUser(_twoButtonsContext, user.Login, user.Password, user.Age, (int) user.SexType,
        user.City, user.Phone, user.Description, user.FullAvatarLink, user.SmallAvatarLink, (int) role, out var userId))
        return Ok("Account created");
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [Authorize]
    [HttpPost("getUserInfoAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      if (!AccountWrapper.TryGetUserInfo(_twoButtonsContext, userId, userPage.UserPageId, out var userInfo))
        return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      if (!AccountWrapper.TryGetUserStatistics(_twoButtonsContext, userPage.UserPageId, out var userStatistics))
        return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      var userContacts =  _accountDb.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();
      
      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return Ok(result);
    }

    [HttpPost("getUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromBody] UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      if (!AccountWrapper.TryGetUserInfo(_twoButtonsContext, userPage.UserId, userPage.UserPageId, out var userInfo))
        return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      if (!AccountWrapper.TryGetUserStatistics(_twoButtonsContext, userPage.UserPageId, out var userStatistics))
        return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      var userContacts = _accountDb.Users.GetUserSocialsAsync(userPage.UserPageId);

      var result = userInfo.MapToUserInfoViewModel();
      result.UserStatistics = userStatistics.MapToUserStatisticsViewModel();

      result.Social = ConvertContactsDtoToViewModel(await userContacts);

      return Ok(result);
    }

    [HttpPost("isUserIdValid")]
    public IActionResult IsUserIdValid([FromBody]UserIdValidationViewModel user)
    {
      if (!AccountWrapper.TryIsUserIdValid(_twoButtonsContext, user.UserId, out var isValid))
        return BadRequest("Sorry, we can not get information from our database.");
      return Ok(new { IsValid = isValid});
    }

    private List<UserContactsViewModel> ConvertContactsDtoToViewModel(UserContactsDTO userContacts)
    {
      var result = new List<UserContactsViewModel>();

      if (userContacts.VkId != 0) result.Add(new UserContactsViewModel{SocialNetType = SocialNetType.VK, AccountUrl = "https://vk.com/id" + userContacts.VkId.ToString()});
      if (userContacts.FacebookId != 0) result.Add(new UserContactsViewModel { SocialNetType = SocialNetType.Facebook, AccountUrl = "https://www.facebook.com/profile.php?id="+userContacts.FacebookId.ToString() });

      return result;
    }

  }
}