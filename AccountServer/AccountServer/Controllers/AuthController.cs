using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Data;
using AccountServer.Entities;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  //[Route("/account")]
  public class AuthController : Controller
  {
    //some config in the appsettings.json
    private JwtIssuerOptions _jwtOptions;

    private IJwtFactory _jwtFactory;
    //repository to handler the sqlite database


    private IMemoryCache _cache;

    private TwoButtonsContext _dbMain;
    private AuthenticationRepository _dbToken;

    public AuthController(IOptions<JwtIssuerOptions> settings, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, TwoButtonsContext context, AuthenticationRepository repository)
    {
      _jwtOptions = settings.Value;
      _dbToken = repository;
      // _cache = memoryCache;
      _jwtFactory = jwtFactory;
      _dbMain = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody]CredentialsViewModel credentials)
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
      int expiresAccessTokenInTime = 5;

      int userId = 0;
      var role = RoleType.Guest;

      if (credentials.GrantType == GrantType.NoGrantType)
      {
        if (string.IsNullOrEmpty(credentials.Login) || string.IsNullOrEmpty(credentials.Password))
          return BadRequest("Invalid username or password.");

        if (!AccountWrapper.TryGetIdentification(_dbMain, credentials.Login, credentials.Password.Trim(),
              out userId) || userId == -1)
          return BadRequest("Sorry, we can not find such login and password in out database.");

        if (!AccountWrapper.TryGetUserRole(_dbMain, userId, out var roleDb))
          return BadRequest("Sorry, we can not find role for the user.");
        role = (RoleType)roleDb;
      }


      if (!string.IsNullOrEmpty(credentials.SecretKey) &&
          !_dbToken.TryFindClient(credentials.ClientId, credentials.SecretKey, out ClientDb client)) { }
      else
      {
        int expiresRefreshTokenInTime;
        switch (role)
        {
          case RoleType.Guest:
            expiresRefreshTokenInTime = 60 * 24 * 7 * 4; // 1 month
            break;
          case RoleType.User when credentials.IsRememberMe:
          case RoleType.Moderator when credentials.IsRememberMe:
          case RoleType.Admin when credentials.IsRememberMe:
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
        await _dbToken.AddClient(client);
      }

      if (!client.IsActive)
      {
        client.IsActive = true;
        await _dbToken.UpdateClient(client);
      }

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = userId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      if (!await _dbToken.AddToken(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");


      _jwtOptions.ValidFor = TimeSpan.FromMinutes(expiresAccessTokenInTime);
      return Ok(Tokens.GenerateJwt(_jwtFactory,client.ClientId, client.Secret, token.RefreshToken,token.UserId, role, _jwtOptions));

    }

    private async Task<IActionResult> RefreshToken(CredentialsViewModel credentials)
    {
      var nowTime = DateTime.UtcNow;

      if (string.IsNullOrEmpty(credentials.RefreshToken) || string.IsNullOrEmpty(credentials.SecretKey))
        return BadRequest("RefreshToken or SecretKey is null or empty. Please, send again.");

      if (!_dbToken.TryFindClient(credentials.ClientId, credentials.SecretKey, out var client) || !client.IsActive)
      {
        return BadRequest("Sorry, you have not loge in yet or your connection with authorization server is expired. Plese, get access token again");
      }

      var oldToken = _dbToken.GetToken(credentials.ClientId, credentials.RefreshToken);
      if (oldToken == null)
      {
        return BadRequest("Sorry, we can not find your \"refresh token\". Plese, get access token again.");
      }

      RoleType role = RoleType.Guest;
      if (oldToken.UserId != 0)
      {
        AccountWrapper.TryGetUserRole(_dbMain, oldToken.UserId, out var roleDb);
        role = (RoleType)roleDb;
      }

      var refreshToken = Guid.NewGuid().ToString();

      var token = new TokenDb
      {
        UserId = oldToken.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      bool isDeleted =  await _dbToken.RemoveToken(oldToken);
      bool isAdded = await _dbToken.AddToken(token);

      if (!isDeleted || !isAdded)
        return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");

      _jwtOptions.ValidFor = TimeSpan.FromMinutes(client.RefreshTokenLifeTime);
      return Ok(Tokens.GenerateJwt(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role, _jwtOptions));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut([FromBody]LogoutParams logout)
    {
      if (string.IsNullOrEmpty(logout?.SecretKey))
        return BadRequest($"Input parameter  is null");

      if (!_dbToken.TryFindClient(logout.ClientId, logout.SecretKey, out var client) || !client.IsActive)
      {
        return BadRequest("You are allready out of the system.");
      }
      client.IsActive = false;
      var isInActive = await _dbToken.UpdateClient(client);
      if (isInActive && !client.IsActive)
        return Ok("Account is out of the system");
      return BadRequest("Your account out of the system");
    }

    [Authorize]
    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut()
    {
      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var tokens = _dbToken.GetAllTokens();
      await _dbToken.RemoveTokens(tokens.Where(x => x.UserId == userId));
      return Ok("You are not in your devices");
    }
  }
}
