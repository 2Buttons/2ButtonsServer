﻿using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IInternalAuthService
  {
    void Dispose();
    Task<UserDto> GetUserByCredentils(LoginViewModel credentials);
    bool IsTokenValid(string token);
    Task<Token> RegisterAsync(UserRegistrationViewModel user);
    Task<bool> ResetPassword(string token, string email, string passwordHash);
    Task<bool> SendConfirmation(int userId);
    Task<bool> SendForgotPassword(string email);
    Task<bool> TryConfirmEmail(int userId, string token);
  }
}