﻿using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationServer.Models;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface ICommonAuthService
  {
    void Dispose();
    Task<bool> FullLogOutAsync(int userId);
    Task<Token> GetAccessTokenAsync(UserDto user);
    Task<Token> GetRefreshTokenAsync(string refreshToken);
    Task<bool> IsUserIdValidAsync(int userId);
    bool IsValidToken(string token);
    Task<bool> LogOutAsync(int userId, string refreshToken);
  }
}