using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Infrastructure.Jwt;
using AuthorizationServer.Models;
using CommonLibraries;
using Microsoft.Extensions.Logging;

namespace AuthorizationServer.Infrastructure.Services
{
  public class CommonAuthService : ICommonAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IJwtService _jwtService;
    private readonly ILogger<CommonAuthService> _logger;

    public CommonAuthService(IJwtService jwtService, AuthorizationUnitOfWork db, ILogger<CommonAuthService> logger)
    {
      _db = db;
      _jwtService = jwtService;
      _logger = logger;
    }


    public async Task<bool> IsEmailFree(string email)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsEmailFree)}.Start");
      var result =  ! await _db.Users.IsUserExistByEmailAsync(email) && ! await _db.Socials.IsUserExistByEmailAsync(email);
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsEmailFree)}.End");
      return result;
    }

    public async Task<Token> GetAccessTokenAsync(UserDto user)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetAccessTokenAsync)}.Start");
      if ((user.RoleType != RoleType.Guest || user.UserId != 0) && await _db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
      {
        await _db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
        throw new Exception("You logged in at least 10 different devices. We are forced to save your data. Now you are logged out of all devices. Please log in again.");
      }

      var result = await _jwtService.GenerateJwtAsync(user.UserId, user.RoleType);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetAccessTokenAsync)}.End");
      return result;
    }

    public async Task<Token> GetRefreshTokenAsync(string refreshToken)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetRefreshTokenAsync)}.Start");
      var tokenFromClient = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
      if (!int.TryParse(
            tokenFromClient.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        throw new Exception("We can not find your id.");
      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) throw new Exception("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        throw new Exception("We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      if (!IsValidToken(tokenFromDb.RefreshToken))
        throw new Exception("The refresh token is invalid. Please, login again.");

      var oldTokenFromDb = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromDb.RefreshToken);
      if (DateTime.UtcNow > oldTokenFromDb.ValidTo)
        throw new Exception("The refresh token is invalid. Please, login again.");

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
        throw new Exception("Can not add token to database. Please, get access token again or enter like a guest");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetRefreshTokenAsync)}.End");
      return result;
    }

    public async Task<bool> LogOutAsync(int userId, string refreshToken)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(LogOutAsync)}.Start");
      var tokenFromDb = await _db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) throw new Exception("We can not find your refresh token. Please, log in again.");
        await _db.Tokens.RemoveTokensByUserIdAsync(userId);
        throw new Exception("We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }
     
      var result =  await _db.Tokens.RemoveTokenAsync(tokenFromDb);
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(LogOutAsync)}.End");
      return result;
    }

    public async Task<bool> FullLogOutAsync(int userId)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(FullLogOutAsync)}.Start");
      var result =  await _db.Tokens.RemoveTokensByUserIdAsync(userId);
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(FullLogOutAsync)}.End");
      return result;
    }

    public async Task<bool> IsUserIdValidAsync(int userId)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsUserIdValidAsync)}.Start");
      var result =  await _db.Users.IsUserIdExistAsync(userId);
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsUserIdValidAsync)}.End");
      return result;
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

    public async Task<UserInfoDb> GetUserInfo(int userId)
    {
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetUserInfo)}.Start");
      var result =  await _db.UsersInfo.GetUserInfoAsync(userId);
      _logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetUserInfo)}.End");
      return result;
    }

    public void Dispose()
    {
      _db.Dispose();
    }
  }
}
