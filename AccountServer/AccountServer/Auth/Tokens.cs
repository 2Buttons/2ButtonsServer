using System.Threading.Tasks;
using AccountServer.Models;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries;
using CommonLibraries.Extensions;

namespace AccountServer.Auth
{
  public class Tokens
  {
    public static async Task<TokenViewModel> GenerateJwtAsync( IJwtFactory jwtFactory, int userId, RoleType role,
      JwtSettings jwtOptions)
    {
      var response = new TokenViewModel
      {
        UserId = userId,
        RoleType = role,
        AccessToken = await jwtFactory.GenerateEncodedAccessToken(userId, role),
        ExpiresIn = jwtOptions.ExpirationAccessToken.ToUnixEpochDate(),
        RefreshToken = await jwtFactory.GenerateEncodedRefreshToken(userId, role)
      };

      return response;
    }
  }
}