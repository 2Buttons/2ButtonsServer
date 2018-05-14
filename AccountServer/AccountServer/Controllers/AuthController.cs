using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries;
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
        return BadRequest($"We are not able to add you. Please, say us about it.");

      if (!AccountWrapper.TryAddUser(_twoButtonsContext, userDb.UserId, user.Login, user.BirthDate,  user.SexType,
        user.City, user.Description, user.FullAvatarLink, user.SmallAvatarLink))
      {
        await _accountDb.Users.RemoveUserAsync(userDb.UserId);
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");
      }

      var nowTime = DateTime.UtcNow;
      var expiresAccessTokenInTime = 60 * 24 * 7 * 2;

      var client = new ClientDb
      {
        Secret = Guid.NewGuid().ToString(),
        IsActive = true,
        RefreshTokenLifeTime = expiresAccessTokenInTime
      };
      await _accountDb.Clients.AddClientAsync(client);

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      if (!await _accountDb.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");


      _jwtOptions.ValidFor = TimeSpan.FromMinutes(expiresAccessTokenInTime);
      return Ok(await Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role, _jwtOptions));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)
    {
      if (credentials == null)
        return BadRequest("Input parameter  is null");

      switch (credentials.GrantType)
      {
        case GrantType.NoGrantType:
        case GrantType.Password:
          return await AccessToken(credentials);
        case GrantType.RefreshToken:
          return await RefreshToken(credentials);
        default:
          return BadRequest("Sorry, we can not find such grant type.");
      }
    }

    private async Task<IActionResult> AccessToken(CredentialsViewModel credentials)
    {
      var nowTime = DateTime.UtcNow;
      var expiresAccessTokenInTime = 5;

      var user = new UserDto{UserId = 0};
      var role = RoleType.Guest;

      if (credentials.GrantType == GrantType.Password)
      {
        if (string.IsNullOrEmpty(credentials.Phone) || string.IsNullOrEmpty(credentials.Password))
        return BadRequest("Phone and (or) password is incorrect");

         user = await _accountDb.Users.GetUserByPhoneAndPasswordAsync(credentials.Phone, credentials.Password.GetHashString());
        if (user == null)
        return BadRequest("Please register or login via Social Network");

         role  = await _accountDb.Users.GetUserRoleAsync(user.UserId);
      }

      ClientDb client = null;
      if (!string.IsNullOrEmpty(credentials.SecretKey))
        client = await _accountDb.Clients.FindClientAsync(credentials.ClientId, credentials.SecretKey);

      if (client == null)
      {
        int expiresRefreshTokenInTime;
        switch (role)
        {
          case RoleType.Guest:
            expiresRefreshTokenInTime = 60 * 24 * 7 * 4; // 1 month
            break;
          case RoleType.User:
          case RoleType.Moderator:
          case RoleType.Admin:
            expiresRefreshTokenInTime = 60 * 24 * 7 * 2; //2 weeks
            break;
          default:
            expiresRefreshTokenInTime = 120; //2 hours
            break;
        }

        client = new ClientDb
        {
          Secret = Guid.NewGuid().ToString(),
          IsActive = true,
          RefreshTokenLifeTime = expiresRefreshTokenInTime
        };
        await _accountDb.Clients.AddClientAsync(client);
      }
      if (!client.IsActive)
      {
        client.IsActive = true;
        await _accountDb.Clients.UpdateClientAsync(client);
      }

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = user.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      if (!await _accountDb.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");


      _jwtOptions.ValidFor = TimeSpan.FromMinutes(expiresAccessTokenInTime);
      return Ok(await Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role,
        _jwtOptions));
    }

    private async Task<IActionResult> RefreshToken(CredentialsViewModel credentials)
    {
      var nowTime = DateTime.UtcNow;

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
    public async Task<IActionResult> LogOut([FromBody] LogoutParams logout)
    {
      if (string.IsNullOrEmpty(logout?.SecretKey))
        return BadRequest($"Input parameter  is null");

      if((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) == RoleType.Guest)
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