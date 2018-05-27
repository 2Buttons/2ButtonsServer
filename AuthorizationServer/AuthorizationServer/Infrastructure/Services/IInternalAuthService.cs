using System;
using System.Threading.Tasks;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IInternalAuthService : IDisposable
  {
    Task<Token> GetAccessTokenAsync(LoginViewModel credentials);
    Task<Token> RegisterAsync(UserRegistrationViewModel user);
  }
}