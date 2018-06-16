using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Infrastructure.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("auth")]
  public class InternalAuthController : Controller
  {
    private readonly IInternalAuthService _internalAuthService;
    private readonly ICommonAuthService _commonAuthService;
    public InternalAuthController(IInternalAuthService internalAuthService, ICommonAuthService commonAuthService)
    {
      _internalAuthService = internalAuthService;
      _commonAuthService = commonAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationViewModel user)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      if (user.Phone.IsNullOrEmpty() && user.Email.IsNullOrEmpty())
      {
        ModelState.AddModelError("Contacts", "Phone and email are null or emty");
        return new BadResponseResult(ModelState);
      }

      var result = await _internalAuthService.RegisterAsync(user);

      return new ResponseResult((int)HttpStatusCode.Created, "User was created.", result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel credentials)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      if ((credentials.GrantType == GrantType.Phone || credentials.GrantType == GrantType.Email) &&
          credentials.Password.IsNullOrEmpty())
      {
        ModelState.AddModelError("Password", "Password is null or empty, but grant type is not guest.");
        return new BadResponseResult(ModelState);
      }

      switch (credentials.GrantType)
      {
        case GrantType.Guest:
        case GrantType.Phone:
        case GrantType.Email:
          var user = await _internalAuthService.GetUserByCredentils(credentials);
          var result = await _commonAuthService.GetAccessTokenAsync(user);
          return new OkResponseResult(result);
        default:
          ModelState.AddModelError("GrantType", "Sorry, we can not find such grant type.");
          return new BadResponseResult(ModelState);
      }
    }

    [HttpGet("confirm/email")]
    public async Task<IActionResult> ConfirmEmail(int userId, string token)
    {
      if (userId == 0 || token.IsNullOrEmpty())
        return RedirectPermanent("http://localhost:6001/confirmedFail");

      if (!await _internalAuthService.ConfirmEmail(userId, token))
        return RedirectPermanent("http://localhost:6001/confirmedFail");
      return RedirectPermanent("http://localhost:6001/confirmedSuccess");
    }
  }
}