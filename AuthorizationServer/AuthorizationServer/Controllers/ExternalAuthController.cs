using System.Threading.Tasks;
using AuthorizationServer.Infrastructure.Services;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using AuthorizationServer.ViewModels.OutputParameters.User;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
  //[DisableCors]
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("auth")]
  public class ExternalAuthController : Controller
  {
    private readonly ICommonAuthService _commonAuthService;
    private readonly IExternalAuthService _externalAuthService;

    public ExternalAuthController(IExternalAuthService externalAuthService, ICommonAuthService commonAuthService)
    {
      _externalAuthService = externalAuthService;
      _commonAuthService = commonAuthService;
    }

    //https://habr.com/post/270273/ - lets encrypt

    [HttpPost("externalLogin")]
    public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginViewModel auth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (auth.State != "S5ocialCode!129_Code")
      {
        ModelState.AddModelError("State", "You are hacker! Your state is incorrect.");
        return new BadResponseResult(ModelState);
      }
      if (!auth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + auth.Error + " " + auth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(auth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");
        return new BadResponseResult(ModelState);
      }

      var userDto = await _externalAuthService.GetUserViaExternalSocialNet(auth.Code, auth.SocialType, auth.IsTest);
      var result = await _commonAuthService.Login(userDto);
      return new OkResponseResult(result);
    }

    [HttpPost("externalLogin/mobile")]
    public async Task<IActionResult> ExternalLoginMobile([FromBody] ExternalLoginMobileViewModel auth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (auth.State != "S5ocialCode!129_Code")
      {
        ModelState.AddModelError("State", "You are hacker! Your state is incorrect.");
        return new BadResponseResult(ModelState);
      }
      var userDto = await _externalAuthService.GetUserViaExternalSocialNet(auth.ExternalUserId,auth.Email,auth.AccessToken,auth.ExpiresIn, auth.SocialType);
      var result = await _commonAuthService.Login(userDto);
      return new OkResponseResult(result);
    }
  }
}