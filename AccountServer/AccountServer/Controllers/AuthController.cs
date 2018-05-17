using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Models;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsAccountDatabase.Entities;
using TwoButtonsAccountDatabase.Repostirories;
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

    public AuthController(IJwtFactory jwtFactory,
      IOptions<JwtIssuerOptions> jwtOptions, TwoButtonsContext twoButtonsContext, AccountUnitOfWork accountDb)
    {
      _jwtOptions = jwtOptions.Value;
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
      var user = new UserDto {UserId = 0};
      var role = RoleType.Guest;

      if (credentials.GrantType == GrantType.Password)
      {
        if (credentials.Phone.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
          return BadRequest("Phone and (or) password is incorrect");
        var passwordHash = credentials.Password.GetHashString();
        user = await _accountDb.Users.GetUserByPhoneAndPasswordAsync(credentials.Phone, passwordHash);
        if (user == null)
          return BadRequest("Please, register or login via Social Network");
        role = await _accountDb.Users.GetUserRoleAsync(user.UserId);

        if (await _accountDb.Tokens.CountTokensForUserAsync(user.UserId) > 10)
        {
          await _accountDb.Tokens.RemoveTokensByUserIdAsync(user.UserId);
          return BadRequest(
            "Your login at leat on 10 defferent devices. We are forced to save your data. Now you are out of all devices. Please log in again.");
        }
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
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return Ok(result);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody]RefreshViewModel refresh)
    {
      if (refresh == null || refresh.RefreshToken.IsNullOrEmpty())
        return BadRequest("Input parameters or is null");
      var tokenFromClient = new JwtSecurityTokenHandler().ReadJwtToken(refresh.RefreshToken);
      if (!int.TryParse(
            tokenFromClient.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return BadRequest("We can not find your id.");
      var tokenFromDb = await _accountDb.Tokens.FindTokenByTokenAndUserIdAsync(userId, refresh.RefreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) return BadRequest("We can not find your refresh token. Please, log in again.");
        await _accountDb.Tokens.RemoveTokensByUserIdAsync(userId);
        return BadRequest(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      if (!IsValidToken(tokenFromDb.RefreshToken))
        return BadRequest("The refresh token is invalid. Please, login again.");

      var oldTokenFromDb = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromDb.RefreshToken);
      if (DateTime.UtcNow > oldTokenFromDb.ValidTo)
        return BadRequest("The refresh token is invalid. Please, login again.");

      var role = userId == 0 ?  RoleType.Guest : await _accountDb.Users.GetUserRoleAsync(userId);

      var result = await Tokens.GenerateJwtAsync(_jwtFactory, userId, role, _jwtOptions);

      var token = new TokenDb
      {
        UserId = userId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      var isDeleted = await _accountDb.Tokens.RemoveTokenAsync(tokenFromDb.TokenId);
      var isAdded = await _accountDb.Tokens.AddTokenAsync(token);

      if (!isDeleted || !isAdded)
        return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      if ((RoleType) int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) ==
          RoleType.Guest)
        return BadRequest($"You are guest.");

      if (!int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        return BadRequest("We can not find your id.");

      var tokenFromDb = await _accountDb.Tokens.FindTokenByIdAsync(userId);
      if (tokenFromDb == null)
      {
        if (userId == 0) return BadRequest("We can not find your refresh token. Please, log in again.");
        await _accountDb.Tokens.RemoveTokensByUserIdAsync(userId);
        return BadRequest(
          "We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      await _accountDb.Tokens.RemoveTokensByUserIdAsync(userId);

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
      await _accountDb.Tokens.RemoveTokensByUserIdAsync(userId);
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