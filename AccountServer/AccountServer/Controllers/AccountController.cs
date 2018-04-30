using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Route("/account")]
  public class AccountController : Controller
  {
    private readonly TwoButtonsContext _twoButtonsContext;

    public AccountController(TwoButtonsContext context)
    {
      _twoButtonsContext = context;
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

      if (UserWrapper.TryAddUser(_twoButtonsContext, user.Login, user.Password, user.Age, (int) user.SexType,
        user.Phone, user.Description, user.FullAvatarLink, user.SmallAvatarLink, out var userId))
        return Ok(userId);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }


    [HttpPost("checkValidLogin")]
    public IActionResult CheckValidLogin(string login)
    {
      if (LoginWrapper.TryCheckValidLogin(_twoButtonsContext, login, out var isValid))
        return Ok(isValid);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }
  }
}