using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLibraries;

namespace AccountServer.Infrastructure.EmailJwt
{
  public interface IEmailJwtService
  {
    JwtSecurityToken DecodeCode(string token);
    Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role, string code);
    Task<string> GenerateJwtAsync(int userId, RoleType role);
    bool IsTokenValid(string token);
  }
}