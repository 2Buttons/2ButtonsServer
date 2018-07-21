using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationServer.Infrastructure.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("auth")]
  public class CommonAuthController : Controller
  {
    private readonly ICommonAuthService _authService;

    public CommonAuthController(ICommonAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("isEmailFree")]
    public async Task<IActionResult> IsEmailFree([FromBody]EmailViewModel email)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var isFree = await _authService.IsEmailFree(email.Email);
      return new OkResponseResult(new {IsFree  = isFree});
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult((object)"Authorization Server");
    }

    [HttpPost("isUserIdValid")]
    public async Task<IActionResult> IsUserIdValid([FromBody] UserIdValidationViewModel user)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var isValid = await _authService.IsUserIdValidAsync(user.UserId);
      return new OkResponseResult(new { IsValid = isValid });
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshViewModel refresh)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var result = await _authService.GetRefreshTokenAsync(refresh.RefreshToken);
      return new OkResponseResult(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutParams logout)
    {
      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest) return new BadResponseResult($"You are a guest.");

      if (!int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return new ResponseResult((int)HttpStatusCode.NotFound, "We can not find your id.");

      if (await _authService.LogOutAsync(userId, logout.RefreshToken))
        return new OkResponseResult("Account is logged out of the system");
      return new ResponseResult((int)HttpStatusCode.InternalServerError, "Account is NOT logged out of the system");
    }

    [Authorize]
    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut()
    {
      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest) return new BadResponseResult($"You are guest and not able to log out from all devices");

      if (!int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return new ResponseResult((int)HttpStatusCode.NotFound, "We can not find your id.");

      if (await _authService.FullLogOutAsync(userId))
        return new OkResponseResult("You are not in your devices");
      return new ResponseResult((int)HttpStatusCode.InternalServerError, "Account is NOT logged out of the system");
    }
  }
}