using System.Threading.Tasks;
using AccountServer.Models;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries;

namespace AccountServer.Auth
{
  public class Tokens
  {
    public static async Task<TokenViewModel> GenerateJwtAsync(IJwtFactory jwtFactory, int clientId, string secretKey, string refreshToken,  int userId, RoleType role, JwtIssuerOptions jwtOptions)
    {
      return await GenerateJwt(jwtFactory,  clientId,  secretKey,  refreshToken,  userId,  role, jwtOptions);
    }

    public static async Task<TokenViewModel> GenerateJwt( IJwtFactory jwtFactory, int clientId, string secretKey, string refreshToken, int userId, RoleType role,
      JwtIssuerOptions jwtOptions)
    {
      var response = new TokenViewModel
      {
        AccessToken = await jwtFactory.GenerateEncodedToken(userId, role),
        UserId = userId,
        RoleType = role,
        ClientId = clientId,
        SecretKey = secretKey,
        ExpiresIn = (int) jwtOptions.ValidFor.TotalMinutes,
        RefreshToken = refreshToken
      };

      return response;
    }
  }
}