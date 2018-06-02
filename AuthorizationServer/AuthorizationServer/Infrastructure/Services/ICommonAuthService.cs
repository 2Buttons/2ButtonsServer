using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface ICommonAuthService : IDisposable
  {
    Task<bool> SaveEmail(string email);
    Task<bool> FullLogOutAsync(int userId);
    Task<Token> GetAccessTokenAsync(UserDto user);
    Task<Token> GetRefreshTokenAsync(string refreshToken);
    Task<bool> IsUserIdValidAsync(int userId);
    Task<UserInfoDb> GetUserInfo(int userId);
    bool IsValidToken(string token);
    Task<bool> LogOutAsync(int userId, string refreshToken);
  }
}