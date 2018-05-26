using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountData;
using AccountData.Account.DTO;
using AccountServer.Infrastructure.Services;
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
    private readonly IAccountService _account;

    public AccountController(IAccountService accountService)
    {
      _account = accountService;
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult((object) "Account");
    }

    [Authorize]
    [HttpGet("user/{userPageId:int}")]
    public async Task<IActionResult> GetUser(int userPageId)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var result = await  _account.GetUserAsync(userId, userPageId);
      return new OkResponseResult(result);
    }

    [Authorize]
    [HttpPost("getUserInfoAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserPageIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var result = await _account.GetUserAsync(userId, userPage.UserPageId);

      return new OkResponseResult(result);
    }

    [HttpPost("getUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromBody] UserPageIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var result = await _account.GetUserAsync(userPage.UserId, userPage.UserPageId);

      return new OkResponseResult(result);
    }
  }
}