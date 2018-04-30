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


    //[HttpPost("clientRegister")]
    //public IActionResult RegisterClient([FromBody]ClientRegistrationViewModel client)
    //{
    //  if (user == null)
    //    return BadRequest($"Input parameter  is null");
    //  if (!ModelState.IsValid)
    //    return BadRequest(ModelState);

    //  if (UserWrapper.TryAddUser(_twoButtonsContext, user.Login, user.Password, user.Age, (int)user.SexType, user.Phone, user.Description, user.FullAvatarLink, user.SmallAvatarLink, out var userId))
    //    return Ok(userId);
    //  return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    //}

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginViewModel login)
    {
      var t = _dbToken.TryFindClient(1, "Secret", out var p);
      return Ok(p);
      if (login == null)
        return BadRequest($"Input parameter  is null");
      //if (!ModelState.IsValid)
      //  return BadRequest(ModelState);

      var userLogin = login.Login.Trim();

      if (string.IsNullOrEmpty(userLogin) || !LoginWrapper.TryCheckValidLogin(_dbMain, userLogin, out var isValid) || !isValid)
        return BadRequest("Invalid username or password.");

      switch (login.GrantType.ToLower())
      {
        case "password":
          return await AccessToken(login);
        case "refreshtoken":
          return await RefreshToken(login);
        default:
          return BadRequest();
      }


    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOut([FromBody]LoginViewModel login)
    {
      if (login == null)
        return BadRequest($"Input parameter  is null");
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var userLogin = login.Login.Trim();

      if (string.IsNullOrEmpty(userLogin) || !LoginWrapper.TryCheckValidLogin(_dbMain, userLogin, out var isValid) || !isValid)
        return BadRequest("Invalid username or password.");

      if (!_dbToken.TryFindClient(login.ClientId, login.Secret, out var client) || !client.IsActive)
      {
        return BadRequest("You are allready out of the system.");
      }

      var token = _dbToken.GetToken(login.ClientId, login.UserId, login.RefreshToken);

      if (token == null)
      {
        return BadRequest("Your account out of the system");
      }

      if(!await _dbToken.RemoveToken(token))
        return BadRequest("Your account out of the system");

      return Ok("Account is oot of the system");
    }

    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut([FromBody]LoginViewModel login)
    {

      if (login == null)
        return BadRequest($"Input parameter  is null");
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var userLogin = login.Login.Trim();

      if (string.IsNullOrEmpty(userLogin) || !LoginWrapper.TryCheckValidLogin(_dbMain, userLogin, out var isValid) || !isValid)
        return BadRequest("Invalid username or password.");


      var tokens = _dbToken.GetAllTokens();
      foreach (var token in tokens)
      {
        await _dbToken.RemoveToken(token);
      }

      return Ok("You are not in your devices");
    }


    private async Task<IActionResult> AccessToken([FromBody]LoginViewModel login)
    {
      if (string.IsNullOrEmpty(login.Password) && login.GrantType == "password")
        return BadRequest("Invalid username or password.");

      if (!LoginWrapper.TryGetIdentification(_dbMain, login.Login, login.Password.Trim(),
            out var userId) || userId == -1)
        return BadRequest("You are not in the system");


      if (!_dbToken.TryFindClient(login.ClientId, login.Secret, out var client))
      {
        client = new Client
        {
          Secret = Guid.NewGuid().ToString(),
          IsActive = true,
          RefreshTokenLifeTime = GetMinutesToResreshToken(2)
        };
        await _dbToken.AddClient(client);
      }

      if (!client.IsActive)
      {
        client.IsActive = true;
        await _dbToken.UpdateClient(client);
      }


      var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

      var token = new Token
      {
        UserId = userId,
        ClientId = client.ClientId,
        IssuedUtc = DateTime.UtcNow,
        ExpiresUtc = DateTime.UtcNow.AddMinutes(1),
        RefreshToken = refreshToken
      };

      if (!ModeratorWrapper.TryGetUserRole(_dbMain, userId, out var role))
      {
        role = 0;
      }

      var userRole = ((UserRole)role).RoleToString();

      //store the refresh_token 
      if (!await _dbToken.AddToken(token))
        return BadRequest("Can not add token to database");
      return Ok(GetJwt(token.UserId, login.Login, userRole, 2, refreshToken));

    }

    private async Task<IActionResult> RefreshToken(LoginViewModel login)
    {

      if (! _dbToken.TryFindClient(login.ClientId, login.Secret, out var client) || !client.IsActive)
      {
        return BadRequest("You are not in the system or your client is inactive. Plese, get access token again");
      }


        var token = _dbToken.GetToken(login.ClientId, login.UserId, login.RefreshToken);

      if (token == null)
      {
        return BadRequest("Can not refresh token. Please, get Access Token");
      }

      if (token.ExpiresUtc.CompareTo(DateTime.UtcNow)>=0)
      {
        return BadRequest("Refresh token has expired. Please, get Access Token again");
      }

      var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

      //expire the old refresh_token and add a new refresh_token
      var updateFlag = await _dbToken.RemoveToken(token);

      var newToken = new Token
      {
        UserId = login.UserId,
        ClientId = client.ClientId,
        IssuedUtc = DateTime.UtcNow,
        ExpiresUtc = DateTime.UtcNow.AddMinutes(client.RefreshTokenLifeTime),
        RefreshToken = refreshToken
      };

      if (!ModeratorWrapper.TryGetUserRole(_dbMain, login.UserId, out var role))
      {
        role = 0;
      }

      var userRole = ((UserRole)role).RoleToString();

      //store the refresh_token 
      if (!await _dbToken.AddToken(newToken) || !updateFlag)
        return BadRequest("Can not add token to database");
      return Ok(GetJwt(token.UserId, login.Login, userRole, (int)client.RefreshTokenLifeTime, refreshToken));
    }

    private string GetJwt(int userId, string login, string role, int expireTime, string refreshToken)
    {
      var now = DateTime.UtcNow;

      //var claims = new Claim[]
      //{
      //    new Claim(JwtRegisteredClaimNames.Sub, clientId),
      //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      //    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
      //};

      var indentity = GetIdentity(userId, login, role);

      var jwt = new JwtSecurityToken(
        issuer: _settings.Value.Issuer,
        audience: _settings.Value.Audience,
        claims: indentity.Claims,
        notBefore: now,
        expires: now.Add(TimeSpan.FromMinutes(expireTime)),
        signingCredentials: GetSigningCredentials());
      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      var response = new
      {
        access_token = encodedJwt,
        expires_in = (int)TimeSpan.FromMinutes(_settings.Value.Lifetime).TotalSeconds,
        refresh_token = refreshToken,
      };

      return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
    }

    private ClaimsIdentity GetIdentity(int userId, string login, string role)
    {

      var claims = new List<Claim>
      {
        new Claim("UserId", userId.ToString(), ClaimValueTypes.Integer32, _settings.Value.Issuer),
        new Claim(ClaimsIdentity.DefaultNameClaimType, login),
        new Claim("Role", role,ClaimValueTypes.String, _settings.Value.Issuer)
      };
      ClaimsIdentity claimsIdentity =
        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
          ClaimsIdentity.DefaultRoleClaimType);
      return claimsIdentity;
    }

    private SigningCredentials GetSigningCredentials()
    {
      var symmetricKeyAsBase64 = _settings.Value.Key;
      var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
      var signingKey = new SymmetricSecurityKey(keyByteArray);

      return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    }

    private long GetMinutesToResreshToken(int days)
    {
      return 10 * 24 * 60;
    }
  }
}
