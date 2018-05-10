using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Models;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries;
using Newtonsoft.Json;

namespace AccountServer.Helpers
{
  public class Tokens
  {
    public static async Task<string> GenerateJwtAsync(IJwtFactory jwtFactory, int clientId, string secretKey, string refreshToken,  int userId, RoleType role, JwtIssuerOptions jwtOptions)
    {
      return await GenerateJwt(jwtFactory,  clientId,  secretKey,  refreshToken,  userId,  role, jwtOptions,
        new JsonSerializerSettings {Formatting = Formatting.Indented});
    }

    public static async Task<string> GenerateJwt( IJwtFactory jwtFactory, int clientId, string secretKey, string refreshToken, int userId, RoleType role,
      JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
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

      return JsonConvert.SerializeObject(response, serializerSettings);
    }
  }
}