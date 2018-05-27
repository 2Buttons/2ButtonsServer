﻿using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IInternalAuthService : IDisposable
  {
    Task<UserDto> GetUserByCredentils(LoginViewModel credentials);
    Task<Token> RegisterAsync(UserRegistrationViewModel user);
  }
}