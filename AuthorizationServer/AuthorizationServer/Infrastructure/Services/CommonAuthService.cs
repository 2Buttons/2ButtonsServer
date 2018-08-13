using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Dto;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Infrastructure.Jwt;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.OutputParameters;
using AuthorizationServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Entities.Main;
using CommonLibraries.MediaFolders;
using Microsoft.Extensions.Logging;

namespace AuthorizationServer.Infrastructure.Services
{
  public class CommonAuthService : ICommonAuthService
  {
    private  AuthorizationUnitOfWork Db { get; }
    private  IJwtService JwtService { get; }
    private  ILogger<CommonAuthService> Logger { get; }
    private  MediaConverter MediaConverter { get; }

    public CommonAuthService(IJwtService jwtService, AuthorizationUnitOfWork db, ILogger<CommonAuthService> logger, MediaConverter  mediaConverter)
    {
      Db = db;
      JwtService = jwtService;
      Logger = logger;
      MediaConverter = mediaConverter;
    }


    public async Task<bool> IsEmailFree(string email)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsEmailFree)}.Start");
      var result =  ! await Db.Users.IsUserExistByEmailAsync(email) && ! await Db.Socials.IsUserExistByEmailAsync(email);
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsEmailFree)}.End");
      return result;
    }

    public async Task<LoginPairViewModel> Login(UserDto user)
    {
      var token = await GetAccessTokenAsync(user);
      var userInfo = user.RoleType == RoleType.Guest? new UserInfoDto() :  await  Db.UsersInfo.GetUserInfoWithCityAsync(user.UserId);
      return new LoginPairViewModel { Token = token, User = UserInfoViewModel.CreateFromUserInfoDb(userInfo, MediaConverter)};
    }

    public async Task<Token> GetAccessTokenAsync(UserDto user)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetAccessTokenAsync)}.Start");
      if ((user.RoleType != RoleType.Guest || user.UserId != 0) && await Db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
      {
        await Db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
        throw new Exception("You logged in at least 10 different devices. We are forced to save your data. Now you are logged out of all devices. Please log in again.");
      }

      var result = await JwtService.GenerateJwtAsync(user.UserId, user.RoleType);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await Db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetAccessTokenAsync)}.End");
      return result;
    }

    public async Task<Token> GetRefreshTokenAsync(string refreshToken)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetRefreshTokenAsync)}.Start");
      var tokenFromClient = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
      if (!int.TryParse(
            tokenFromClient.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "-1",
            out var userId) || userId == -1)
        throw new Exception("We can not find your id.");
      var tokenFromDb = await Db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) throw new Exception("We can not find your refresh token. Please, log in again.");
        await Db.Tokens.RemoveTokensByUserIdAsync(userId);
        throw new Exception("We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }

      if (!IsValidToken(tokenFromDb.RefreshToken))
        throw new Exception("The refresh token is invalid. Please, login again.");

      var oldTokenFromDb = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromDb.RefreshToken);
      if (DateTime.UtcNow > oldTokenFromDb.ValidTo)
        throw new Exception("The refresh token is invalid. Please, login again.");

      var role = userId == 0 ? RoleType.Guest : await Db.Users.GetUserRoleAsync(userId);

      var result = await JwtService.GenerateJwtAsync(userId, role);

      var token = new TokenDb
      {
        UserId = userId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      var isDeleted = await Db.Tokens.RemoveTokenAsync(tokenFromDb.TokenId);
      var isAdded = await Db.Tokens.AddTokenAsync(token);

      if (!isDeleted || !isAdded)
        throw new Exception("Can not add token to database. Please, get access token again or enter like a guest");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(GetRefreshTokenAsync)}.End");
      return result;
    }

    public async Task<bool> LogOutAsync(int userId, string refreshToken)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(LogOutAsync)}.Start");
      var tokenFromDb = await Db.Tokens.FindTokenByTokenAndUserIdAsync(userId, refreshToken);
      if (tokenFromDb == null)
      {
        if (userId == 0) throw new Exception("We can not find your refresh token. Please, log in again.");
        await Db.Tokens.RemoveTokensByUserIdAsync(userId);
        throw new Exception("We can not find your refresh token. Please, login again We are forced to save your data. Now you are out of all devices. Please log in again.");
      }
     
      var result =  await Db.Tokens.RemoveTokenAsync(tokenFromDb);
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(LogOutAsync)}.End");
      return result;
    }

    public async Task<bool> FullLogOutAsync(int userId)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(FullLogOutAsync)}.Start");
      var result =  await Db.Tokens.RemoveTokensByUserIdAsync(userId);
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(FullLogOutAsync)}.End");
      return result;
    }

    public async Task<bool> IsUserIdValidAsync(int userId)
    {
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsUserIdValidAsync)}.Start");
      var result =  await Db.Users.IsUserIdExistAsync(userId);
      Logger.LogInformation($"{nameof(CommonAuthService)}.{nameof(IsUserIdValidAsync)}.End");
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

 

    public void Dispose()
    {
      Db.Dispose();
    }
  }
}
