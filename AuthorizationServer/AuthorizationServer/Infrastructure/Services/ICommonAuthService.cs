using System;
using System.Threading.Tasks;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters.Auth;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface ICommonAuthService : IDisposable
  {
    Task<Token> GetRefreshTokenAsync(string refreshToken);
    Task<bool> IsUserIdValidAsync(int userId);
    Task<bool> LogOutAsync(int userId, string refreshToken);
    Task<bool> FullLogOutAsync(int userId);
  }
}