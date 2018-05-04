using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountServer.Entities;
using AccountServer.Models;
using AccountServer.Repositories;
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
  public class TokenController : Controller
  {
    //some config in the appsettings.json
    private IOptions<AuthenticationOptions> _settings;

    //repository to handler the sqlite database


    private IMemoryCache _cache;

    private TwoButtonsContext _dbMain;
    private AuthenticationRepository _dbToken;

    public TokenController(IOptions<AuthenticationOptions> settings, TwoButtonsContext context, AuthenticationRepository repository)
    {
      _settings = settings;
      _dbToken = repository;
      // _cache = memoryCache;
      _dbMain = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody]LoginViewModel login)
    {
      if (login == null)
        return BadRequest("Input parameter  is null");

      switch (login.GrantType)
      {
        case GrantType.Guest:
          return await GuestToken();
        case GrantType.Password:
          return await AccessToken(login);
        case GrantType.RefreshToken:
          return await RefreshToken(login);
        default:
          return BadRequest("Sorry, we can not find such grant type.");
      }
    }

    private async Task<IActionResult> GuestToken()
    {
      var nowTime = DateTime.UtcNow;
      int expiresInTime = 60 * 24 * 7 * 4;

      var userId = 0;
      var login = "";
      var client = new ClientDb
      {
        Secret = Guid.NewGuid().ToString().Replace("-", ""),
        IsActive = true,
        RefreshTokenLifeTime = expiresInTime
      };
      await _dbToken.AddClient(client);

      if (!client.IsActive)
      {
        client.IsActive = true;
        await _dbToken.UpdateClient(client);
      }

      var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

      var token = new TokenDb
      {
        UserId = userId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(expiresInTime),
        RefreshToken = refreshToken
      };

      var userRole = RoleType.Guest.ToString();

      //store the refresh_token 
      if (!await _dbToken.AddToken(token))
        return BadRequest("Can not add token to database");

      return Ok(GetJwt(token.UserId, login, token.ClientId, client.Secret, userRole, nowTime, expiresInTime, refreshToken));
    }


    private async Task<IActionResult> AccessToken(LoginViewModel login)
    {
      var nowTime = DateTime.UtcNow;
      int expiresInTime = 5;

      if (string.IsNullOrEmpty(login.Login) || string.IsNullOrEmpty(login.Password))
        return BadRequest("Invalid username or password.");

      if (!LoginWrapper.TryGetIdentification(_dbMain, login.Login, login.Password.Trim(),
            out var userId) || userId == -1)
        return BadRequest("Sorry, we can not find such login and password in out database.");


      if (!_dbToken.TryFindClient(login.ClientId, login.SecretKey, out var client))
      {
        client = new ClientDb
        {
          Secret = Guid.NewGuid().ToString().Replace("-", ""),
          IsActive = true,
          RefreshTokenLifeTime = expiresInTime
        };
        await _dbToken.AddClient(client);
      }

      if (!client.IsActive)
      {
        client.IsActive = true;
        await _dbToken.UpdateClient(client);
      }

      var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

      var token = new TokenDb
      {
        UserId = userId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(expiresInTime),
        RefreshToken = refreshToken
      };

      if (!ModeratorWrapper.TryGetUserRole(_dbMain, userId, out var role))
      {
        role = 0;
      }

      var userRole = ((RoleType)role).ToString();


      if (!await _dbToken.AddToken(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");
      //return BadRequest(new BadRequestJustGuest
      //{
      //  Message= "Can not add token to database. You entered just as a guest.",
      //  Token = GetJwt(token.UserId, login.Login, token.ClientId, client.Secret, userRole, nowTime, expiresInTime, refreshToken)
      //});
      return Ok(GetJwt(token.UserId, login.Login, token.ClientId, client.Secret, userRole, nowTime, expiresInTime, refreshToken));

    }

    private async Task<IActionResult> RefreshToken(LoginViewModel login)
    {
      var nowTime = DateTime.UtcNow;
      int expiresInTime = 30;

      if (string.IsNullOrEmpty(login.Login) || string.IsNullOrEmpty(login.SecretKey))
        return BadRequest("Invalid username or password.");

      if (!_dbToken.TryFindClient(login.ClientId, login.SecretKey, out var client) || !client.IsActive)
      {
        return BadRequest("Sorry, you have not loge in yet or your connection with authorization server is expired. Plese, get access token again");
      }

      var token = _dbToken.GetToken(login.ClientId, login.RefreshToken);
      if (token == null)
      {
        return BadRequest("Sorry, we can not find your refresh token. Plese, get access token again.");
      }

      if (token.ExpiresUtc.CompareTo(DateTime.UtcNow) < 0)
      {
        return BadRequest("Refresh token has expired. Plese, get access token again.");
      }

      var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

      //expire the old refresh_token and add a new refresh_token
      var updateFlag = await _dbToken.RemoveToken(token);

      var newToken = new TokenDb
      {
        UserId = token.UserId,
        ClientId = client.ClientId,
        IssuedUtc = nowTime,
        ExpiresUtc = nowTime.AddMinutes(expiresInTime),
        RefreshToken = refreshToken
      };

      if (!ModeratorWrapper.TryGetUserRole(_dbMain, token.UserId, out var role))
      {
        role = 0;
      }

      var userRole = ((RoleType)role).ToString();

      //store the refresh_token 
      if (!await _dbToken.AddToken(newToken) || !updateFlag)
        return BadRequest("Can not add token to database");
      return Ok(GetJwt(token.UserId, login.Login, token.ClientId, client.Secret, userRole, nowTime, expiresInTime, refreshToken));
    }

    private string GetJwt(int userId, string login, int clientId, string clientSecret, string role, DateTime nowUtc, int expireTime, string refreshToken)
    {
      var now = nowUtc;
      var expiresIn = now.Add(TimeSpan.FromMinutes(expireTime));

      var indentity = GetIdentity(userId, role);
      var jwt = new JwtSecurityToken(
        issuer: _settings.Value.Issuer,
        audience: _settings.Value.Audience,
        claims: indentity.Claims,
        notBefore: now,
        expires: expiresIn,
        signingCredentials: GetSigningCredentials());
      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      var response = new IdentityRespose
      {
        AccessToken = encodedJwt,
        UserId = userId,
        ClientId = clientId,
        SecretKey = clientSecret,
        ExpiresIn = expireTime,
        RefreshToken = refreshToken,
      };

      return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
    }

    private ClaimsIdentity GetIdentity(int userId, string role)
    {
      var claims = new List<Claim>
      {
     //   new Claim(ClaimsIdentity.., userId.ToString(), ClaimValueTypes.Integer32, _settings.Value.Issuer),
        new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString(), ClaimValueTypes.Integer,  _settings.Value.Issuer),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, role,ClaimValueTypes.String, _settings.Value.Issuer)
      };
      return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }

    private SigningCredentials GetSigningCredentials()
    {
      var symmetricKeyAsBase64 = _settings.Value.SecretKey;
      var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
      var signingKey = new SymmetricSecurityKey(keyByteArray);

      return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
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
