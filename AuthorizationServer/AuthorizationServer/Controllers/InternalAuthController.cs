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
using AuthorizationServer.Services;
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
  //[Route("/auth")]
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

      switch (credentials.GrantType)
      {
        case GrantType.Guest:
        case GrantType.Password:
        case GrantType.Email:
          var user = await _internalAuthService.GetUserByCredentils(credentials);
          var result = await _commonAuthService.GetAccessTokenAsync(user);
          return new OkResponseResult(result);
        default:
          ModelState.AddModelError("GrantType", "Sorry, we can not find such grant type.");
          return new BadResponseResult(ModelState);
      }
    }
  }
}