using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IInternalAuthService
  {
    Task<bool> ConfirmEmail(int uderId, string token);
    void Dispose();
    Task<string> GetConfirmedEmailToken(int userId, RoleType role);
    Task<UserDto> GetUserByCredentils(LoginViewModel credentials);
    Task<Token> RegisterAsync(UserRegistrationViewModel user);
  }
}