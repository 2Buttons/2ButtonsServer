using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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

    [HttpGet("server")]
    public  IActionResult ServerName()
    {
      return new OkResponseResult((object)"Authorization Server");
    }

    [HttpPost("isUserIdValid")]
    public async Task<IActionResult> IsUserIdValid([FromBody] UserIdValidationViewModel user)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var isValid = await _db.Users.IsUserIdExistAsync(user.UserId);
      return new OkResponseResult(new { IsValid = isValid });
    }

    [HttpGet("register")]
    public IActionResult RegisterUser()
    {
      return new BadResponseResult("Please, use POST request.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationViewModel user)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      if (user.Phone.IsNullOrEmpty() && user.Email.IsNullOrEmpty())
      {
        ModelState.AddModelError("Contacts", "Phone and email are null or emty");
        return new BadResponseResult("Validation errors.", ModelState);
      }
      var isExistByPhone = _db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = _db.Users.IsUserExistByPhoneAsync(user.Email);

      await Task.WhenAll(isExistByPhone, isExistByEmail);

      if (isExistByEmail.Result || isExistByPhone.Result)
        return new BadResponseResult($"You have already registered.");

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
        return new BadResponseResult($"We are not able to add you. Please, tell us about it.");

      if (!await _db.Users.AddUserIntoMainDbAsync(userDb.UserId, user.Login, user.BirthDate, user.SexType,
        user.City, user.Description, user.FullAvatarLink, user.SmallAvatarLink))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        return new BadResponseResult("We are not able to add your indformation. Please, tell us about it.");
      }


      var result = await _jwtService.GenerateJwtAsync(userDb.UserId, role);

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        return new BadResponseResult("Can not add token to database. You entered just as a guest.");

      return new ResponseResult(result, StatusCodes.Status201Created, "User was created.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel credentials)
    {
      if (credentials == null)
        return new BadResponseResult("Input parameter  is null");

      switch (credentials.GrantType)
      {
        case GrantType.Guest:
        case GrantType.Password:
        case GrantType.Email:
          return await AccessToken(credentials);
        default:
          return new BadResponseResult("Sorry, we can not find such grant type.");
      }
    }

    private async Task<IActionResult> AccessToken(LoginViewModel credentials)
    {
      var user = new UserDto { UserId = 0 };
      var role = RoleType.Guest;

      if (credentials.GrantType != GrantType.Guest)
      {
        switch (credentials.GrantType)
        {
          case GrantType.Guest:
            break;
          case GrantType.Password:
            if (credentials.Phone.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
              return new BadResponseResult("Phone and (or) password is incorrect");
            user = await _db.Users.GetUserByPhoneAndPasswordAsync(credentials.Phone, credentials.Password);
            break;
          case GrantType.Email:
            if (credentials.Email.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
              return new BadResponseResult("Email and (or) password is incorrect");
            user = await _db.Users.GetUserByEmailAndPasswordAsync(credentials.Email, credentials.Password);
            break;
        }

        if (user == null)
          return new BadResponseResult("Please, register or login via Social Network");
        role = await _db.Users.GetUserRoleAsync(user.UserId);

        if (await _db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
        {
          await _db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
          return new BadResponseResult(
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
        return new BadResponseResult("Can not add token to database. You entered just as a guest.");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return new OkResponseResult(result);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshViewModel refresh)
    {
      if (refresh == null || refresh.RefreshToken.IsNullOrEmpty())
        return new BadResponseResult("Input parameters or is null");
      var tokenFromClient = new JwtSecurityTokenHandler().ReadJwtToken(refresh.RefreshToken);
      if (!int.TryParse(
            tokenFromClient.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return new BadResponseResult("We can not find your id.");
      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refresh.RefreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) return new BadResponseResult("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        return new BadResponseResult(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      if (!IsValidToken(tokenFromDb.RefreshToken))
        return new BadResponseResult("The refresh token is invalid. Please, login again.");

      var oldTokenFromDb = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromDb.RefreshToken);
      if (DateTime.UtcNow > oldTokenFromDb.ValidTo)
        return new BadResponseResult("The refresh token is invalid. Please, login again.");

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
        return new BadResponseResult("Can not add token to database. Plese, get access token again or enter like a guest");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return new OkResponseResult(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]LogoutParams logout)
    {
      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest)
        return new BadResponseResult($"You are guest.");

      if (!int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return new BadResponseResult("We can not find your id.");

      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, logout.RefreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) return new BadResponseResult("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        return new BadResponseResult(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      await _db.Tokens.RemoveTokenAsync(tokenFromDb);

      return new OkResponseResult("Account is logged out of the system");
    }

    [Authorize]
    [HttpPost("fullLogout")]
    public async Task<IActionResult> FullLogOut()
    {
      if ((RoleType)int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest)
        return new BadResponseResult($"You are guest and not able to log out from all devices");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      await _db.Tokens.RemoveTokensByUserIdAsync(userId);
      return new OkResponseResult("You are not in your devices");
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