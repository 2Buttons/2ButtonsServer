using System.Net;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationServer.Infrastructure.Services;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using AuthorizationServer.ViewModels.OutputParameters;
using AuthorizationServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationViewModel = AuthorizationServer.ViewModels.InputParameters.UserRegistrationViewModel;

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
        ModelState.AddModelError("Contacts", "Phone and email are null or empty");
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
      var user = new UserDto { UserId = 0, RoleType = RoleType.Guest };
      switch (credentials.GrantType)
      {
        case GrantType.Guest: break;
        case GrantType.Phone:
          user = await _internalAuthService.GetUserByPhone(credentials.Phone, credentials.Password);
          if (user == null)
            return new ResponseResult((int)HttpStatusCode.Forbidden, "Phone and(or) password is incorrect", new { Token = new Token(), User = new UserInfoViewModel() });
          break;
        case GrantType.Email:
          user = await _internalAuthService.GetUserByEmail(credentials.Email, credentials.Password);
          if (user == null)
            return new ResponseResult((int)HttpStatusCode.Forbidden, "Email and(or) password is incorrect", new { Token = new Token(), User = new UserInfoViewModel() });
          break;
        default:
          ModelState.AddModelError("GrantType", "Sorry, we can not find such grant type.");
          return new BadResponseResult(ModelState);
      }

      var result = await _commonAuthService.Login(user);
      return new OkResponseResult(result);


    }

    //[HttpPost("forgotPassword")]
    //public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
    //{
    //  if (!ModelState.IsValid)
    //  {
    //    return new BadResponseResult(ModelState);
    //  }

    //  if (await _internalAuthService.SendForgotPassword(model.Email)) return new OkResponseResult("Check your email to move to reset page.");
    //  return new ResponseResult((int) HttpStatusCode.Forbidden, "Something wen wrong.");
    //}

    //[HttpPost("resetPassword")]
    //public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
    //{
    //  if (!ModelState.IsValid)
    //  {
    //    return new BadResponseResult(ModelState);
    //  }

    //  if (!_internalAuthService.IsTokenValid(model.Token))
    //  {
    //    ModelState.AddModelError("Token", "Token is not valid");
    //    return new BadResponseResult(ModelState);
    //  }

    //  if (await _internalAuthService.ResetPassword(model.Token, model.Email, model.Password.GetHashString()))
    //  {
    //    await _internalAuthService.SendResetPassword(model.Email);
    //    return new OkResponseResult("Password was reseted.",new {IsReseted = false});
    //  }
    //  return new ResponseResult((int)HttpStatusCode.NotModified, "Password was not reseted", new { IsReseted = true });

    //}

    //[HttpPost("confirmEmail")]
    //public async Task<IActionResult> ConfirmEmail([FromBody]int userId)
    //{
    //  if (!ModelState.IsValid)
    //    return new BadResponseResult(ModelState);
    //  return await _internalAuthService.SendConfirmation(userId) ? new OkResponseResult("We sent the confirmation email") : new ResponseResult((int)HttpStatusCode.NotFound, "We can not fiund this account");
    //}

    //[HttpGet("confirmEmail")]
    //public async Task<IActionResult> ConfirmEmail(int userId, string token)
    //{
    //  if (userId == 0 || token.IsNullOrEmpty() || ! _internalAuthService.IsTokenValid(token))
    //    return RedirectPermanent("http://localhost:11081/confirmFail.html");

    //  if (!await _internalAuthService.TryConfirmEmail(userId, token))
    //    return RedirectPermanent("http://localhost:11081/confirmFail.html");
    //  {
    //    await _internalAuthService.SendCongratilationsThatEmailConfirmed(userId);
    //    return RedirectPermanent("http://localhost:11081/confirmSuccess.html");
    //  }
    //}
  }
}