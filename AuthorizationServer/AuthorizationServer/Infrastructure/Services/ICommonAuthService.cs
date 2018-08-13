using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.OutputParameters;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface ICommonAuthService : IDisposable
  {
    Task<bool> IsEmailFree(string email);
    Task<bool> FullLogOutAsync(int userId);
    Task<Token> GetAccessTokenAsync(UserDto user);
    Task<Token> GetRefreshTokenAsync(string refreshToken);
    Task<LoginPairViewModel> Login(UserDto user);
    Task<bool> IsUserIdValidAsync(int userId);
    bool IsValidToken(string token);
    Task<bool> LogOutAsync(int userId, string refreshToken);
  }
}