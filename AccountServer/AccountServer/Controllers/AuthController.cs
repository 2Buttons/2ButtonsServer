using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsAccountDatabase.Entities;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("/auth")]
  public class AuthController : Controller
  {
    //repository to handler the sqlite database

    private readonly AccountUnitOfWork _accountDb;

    private readonly IJwtFactory _jwtFactory;

    //some config in the appsettings.json
    private readonly JwtIssuerOptions _jwtOptions;

    private readonly TwoButtonsContext _twoButtonsContext;

    public AuthController(IOptions<JwtIssuerOptions> settings, IJwtFactory jwtFactory,
      IOptions<JwtIssuerOptions> jwtOptions, TwoButtonsContext twoButtonsContext, AccountUnitOfWork accountDb)
    {
      _jwtOptions = settings.Value;
      _accountDb = accountDb;
      _jwtFactory = jwtFactory;
      _twoButtonsContext = twoButtonsContext;
    }

    [HttpGet("register")]
    public IActionResult AddUser()
    {
      return BadRequest("Please, use POST request.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationViewModel user)
    {
      if (user == null)
        return BadRequest($"Input parameter  is null");
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      var isExistByPhone = _accountDb.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = _accountDb.Users.IsUserExistByPhoneAsync(user.Email);

      await Task.WhenAll(isExistByPhone, isExistByEmail);

      if (isExistByEmail.Result || isExistByPhone.Result)
        return BadRequest($"You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserDb
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetHashString().GetHashString()
      };
      var isAdded = await _accountDb.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0)
        return BadRequest($"We are not able to add you. Please, tell us about it.");

      if (!AccountWrapper.TryAddUser(_twoButtonsContext, userDb.UserId, user.Login, user.BirthDate, user.SexType,
        user.City, user.Description, user.FullAvatarLink, user.SmallAvatarLink))
      {
        await _accountDb.Users.RemoveUserAsync(userDb.UserId);
        return BadRequest("We are not able to add your indformation. Please, tell us about it.");
      }


      var result = await Tokens.GenerateJwtAsync(_jwtFactory, userDb.UserId, role, _jwtOptions);

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _accountDb.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");

      return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel credentials)
    {
      if (credentials == null)
        return BadRequest("Input parameter  is null");

      switch (credentials.GrantType)
      {
        case GrantType.Guest:
        case GrantType.Password:
          return await AccessToken(credentials);
        default:
          return BadRequest("Sorry, we can not find such grant type.");
      }
    }

    private async Task<IActionResult> AccessToken(LoginViewModel credentials)
    {
      var user = new UserDto { UserId = 0 };
      var role = RoleType.Guest;

      if (credentials.GrantType == GrantType.Password)
      {
        if (credentials.Phone.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
          return BadRequest("Phone and (or) password is incorrect");
        var passwordHash = credentials.Password.GetHashString();
        user = await _accountDb.Users.GetUserByPhoneAndPasswordAsync(credentials.Phone, passwordHash);
        if (user == null)
          return BadRequest("Please register or login via Social Network");
        role = await _accountDb.Users.GetUserRoleAsync(user.UserId);

        if()
      }



      var result = await Tokens.GenerateJwtAsync(_jwtFactory, user.UserId, role, _jwtOptions);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _accountDb.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");

      return Ok(result);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshViewModel refresh)
    {
      var nowTime = DateTime.UtcNow;

      if (refresh == null || refresh.RefreshToken.IsNullOrEmpty())
      {
        return BadRequest("Input parameters or is null");
      }



      if (string.IsNullOrEmpty(credentials.RefreshToken) || string.IsNullOrEmpty(credentials.SecretKey))
        return BadRequest("RefreshToken or SecretKey is null or empty. Please, send again.");

      var client = await _accountDb.Clients.FindClientAsync(credentials.ClientId, credentials.SecretKey);
      if (client == null || !client.IsActive)
        return BadRequest(
          "Sorry, you have not loge in yet or your connection with authorization server is expired. Plese, get access token again");

      var oldToken = await _accountDb.Tokens.FindTokenAsync(credentials.ClientId, credentials.RefreshToken);
      if (oldToken == null)
        return BadRequest("Sorry, we can not find your \"refresh token\". Plese, get access token again.");

      var role = RoleType.Guest;
      if (oldToken.UserId != 0)
        role = await _accountDb.Users.GetUserRoleAsync(oldToken.UserId);

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = oldToken.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      var isDeleted = await _accountDb.Tokens.RemoveTokenAsync(oldToken);
      var isAdded = await _accountDb.Tokens.AddTokenAsync(token);

      if (!isDeleted || !isAdded)
        return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");

      _jwtOptions.ValidFor = TimeSpan.FromMinutes(client.RefreshTokenLifeTime);
      return Ok(await Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role,
        _jwtOptions));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutParams logout)
    {
      if (string.IsNullOrEmpty(logout?.SecretKey))
        return BadRequest($"Input parameter  is null");

      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) == RoleType.Guest)
      {
        return BadRequest($"You are guest.");
      }

      var client = await _accountDb.Clients.FindClientAsync(logout.ClientId, logout.SecretKey);
      if (client == null || !client.IsActive)
        return BadRequest("You are allready out of the system.");
      client.IsActive = false;
      var isInActive = await _accountDb.Clients.UpdateClientAsync(client);
      if (isInActive && !client.IsActive)
        return Ok("Account is out of the system");
      return BadRequest("Your account out of the system");
    }

    [Authorize]
    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut()
    {
      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) == RoleType.Guest)
      {
        return BadRequest($"You are guest.");
      }

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      await _accountDb.Tokens.RemoveTokensAsync(x => x.UserId == userId);
      return Ok("You are not in your devices");
    }
  }
}