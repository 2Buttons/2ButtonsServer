using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationServer.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("/auth")]
  public class AuthController : Controller
  {
    //repository to handler the sqlite database

    private readonly AuthorizationUnitOfWork _db;

    //some config in the appsettings.json
    private readonly IJwtService _jwtService;


    public AuthController(IJwtService jwtService, AuthorizationUnitOfWork db)
    {
      _db = db;
      _jwtService = jwtService;
    }

    [HttpPost("isUserIdValid")]
    public async Task<IActionResult> IsUserIdValid([FromBody] UserIdValidationViewModel user)
    {
      var isValid = await _db.Users.IsUserIdExistAsync(user.UserId);
      return Ok(new { IsValid = isValid });
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
      var isExistByPhone = _db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = _db.Users.IsUserExistByPhoneAsync(user.Email);

      await Task.WhenAll(isExistByPhone, isExistByEmail);

      if (isExistByEmail.Result || isExistByPhone.Result)
        return BadRequest($"You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserDb
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetHashString()
      };
      var isAdded = await _db.Users.AddUserIntoAccountDbAsync(userDb);
      if (!isAdded || userDb.UserId == 0)
        return BadRequest($"We are not able to add you. Please, tell us about it.");

      if (! await _db.Users.AddUserIntoMainDbAsync(userDb.UserId, user.Login, user.BirthDate, user.SexType,
        user.City, user.Description, user.FullAvatarLink, user.SmallAvatarLink))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        return BadRequest("We are not able to add your indformation. Please, tell us about it.");
      }


      var result = await _jwtService.GenerateJwtAsync( userDb.UserId, role);

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
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
        case GrantType.Email:
          return await AccessToken(credentials);
        default:
          return BadRequest("Sorry, we can not find such grant type.");
      }
    }

    private async Task<IActionResult> AccessToken(LoginViewModel credentials)
    {
      var user = new UserDto {UserId = 0};
      var role = RoleType.Guest;

      if (credentials.GrantType != GrantType.Guest)
      {
        switch (credentials.GrantType)
        {
          case GrantType.Guest:
            break;
          case GrantType.Password:
            if (credentials.Phone.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
              return BadRequest("Phone and (or) password is incorrect");
            user = await _db.Users.GetUserByPhoneAndPasswordAsync(credentials.Phone, credentials.Password);
            break;
          case GrantType.Email:
            if (credentials.Email.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
              return BadRequest("Email and (or) password is incorrect");
            user = await _db.Users.GetUserByEmailAndPasswordAsync(credentials.Email, credentials.Password);
            break;
        }

        if (user == null)
          return BadRequest("Please, register or login via Social Network");
        role = await _db.Users.GetUserRoleAsync(user.UserId);

        if (await _db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
        {
          await _db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
          return BadRequest(
            "Your login at leat on 10 defferent devices. We are forced to save your data. Now you are out of all devices. Please log in again.");
        }
      }

      var result = await _jwtService.GenerateJwtAsync(user.UserId, role);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        return BadRequest("Can not add token to database. You entered just as a guest.");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return Ok(result);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshViewModel refresh)
    {
      if (refresh == null || refresh.RefreshToken.IsNullOrEmpty())
        return BadRequest("Input parameters or is null");
      var tokenFromClient = new JwtSecurityTokenHandler().ReadJwtToken(refresh.RefreshToken);
      if (!int.TryParse(
            tokenFromClient.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return BadRequest("We can not find your id.");
      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refresh.RefreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) return BadRequest("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        return BadRequest(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      if (!IsValidToken(tokenFromDb.RefreshToken))
        return BadRequest("The refresh token is invalid. Please, login again.");

      var oldTokenFromDb = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromDb.RefreshToken);
      if (DateTime.UtcNow > oldTokenFromDb.ValidTo)
        return BadRequest("The refresh token is invalid. Please, login again.");

      var role = userId == 0 ? RoleType.Guest : await _db.Users.GetUserRoleAsync(userId);

      var result = await _jwtService.GenerateJwtAsync(userId, role);

      var token = new TokenDb
      {
        UserId = userId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      var isDeleted = await _db.Tokens.RemoveTokenAsync(tokenFromDb.TokenId);
      var isAdded = await _db.Tokens.AddTokenAsync(token);

      if (!isDeleted || !isAdded)
        return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]LogoutParams logout)
    {
      if ((RoleType) int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest)
        return BadRequest($"You are guest.");

      if (!int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return BadRequest("We can not find your id.");

      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, logout.RefreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) return BadRequest("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        return BadRequest(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      await _db.Tokens.RemoveTokenAsync(tokenFromDb);

      return Ok("Account is logged out of the system");
    }

    [Authorize]
    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut()
    {
      if ((RoleType) int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest)
        return BadRequest($"You are guest and not able to log out from all devices");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      await _db.Tokens.RemoveTokensByUserIdAsync(userId);
      return Ok("You are not in your devices");
    }

    public bool IsValidToken(string token)
    {
      return true;
      //try
      //{
      //  new JwtSecurityTokenHandler().ValidateToken(token,
      //    new TokenValidationParameters {ValidateIssuerSigningKey = true}, out var tokenValidation);
      //  return true;
      //}
      //catch (Exception e)
      //{
      //  Console.WriteLine(e);
      //  return false;
      //}
    }
  }
}