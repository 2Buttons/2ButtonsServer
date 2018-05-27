using System.Threading.Tasks;
using AuthorizationServer.Infrastructure.Services;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("external")]
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
      if (auth.State != "123456")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorret");
        return new BadResponseResult(ModelState);
      }
      if (!auth.Status || !auth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + auth.Error + " " + auth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(auth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");
        return new BadResponseResult(ModelState);
      }

      var userDto = await _externalAuthService.GetUserViaExternalSocialNet(auth.Code, auth.SocialType);

      var result = await _commonAuthService.GetAccessTokenAsync(userDto);
      return new OkResponseResult(result);
    }
  }
}