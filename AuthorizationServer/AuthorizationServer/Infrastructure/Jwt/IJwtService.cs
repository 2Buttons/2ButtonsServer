using System.Threading.Tasks;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.OutputParameters;
using CommonLibraries;

namespace AuthorizationServer.Infrastructure.Jwt
{
  public interface IJwtService
  {
    Task<Token> GenerateJwtAsync(int userId, RoleType role);
    //Task<string> GenerateEncodedAccessToken(int userId, RoleType role);
    //Task<string> GenerateEncodedRefreshToken(int userId, RoleType role);
    //Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role);
  }
}
