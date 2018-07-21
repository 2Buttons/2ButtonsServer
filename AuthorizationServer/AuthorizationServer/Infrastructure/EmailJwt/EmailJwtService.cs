using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonTypes;
using Microsoft.Extensions.Options;

namespace AuthorizationServer.Infrastructure.EmailJwt
{
  public class EmailJwtService : IEmailJwtService
  {
    private readonly EmailJwtSettings _jwtSettings;

    public EmailJwtService(IOptions<EmailJwtSettings> jwtOptions)
    {
      _jwtSettings = jwtOptions.Value;
      ThrowIfInvalidOptions(_jwtSettings);
    }

    public async Task<string> GenerateJwtAsync(int userId, RoleType role)
    {
      return await GenerateEncodedTokenAsync(userId, role, _jwtSettings.Expiration);
    }

    private async Task<string> GenerateEncodedTokenAsync(int userId, RoleType role, DateTime expiresTime)
    {
      var claimsIdentity = await GenerateClaimsIdentity(userId, role, _jwtSettings.Code);
      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claimsIdentity.Claims,
        _jwtSettings.NotBefore, expiresTime, _jwtSettings.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }

    public async Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role, string code)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtSettings.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, _jwtSettings.IssuedAt.ToUnixEpochDate().ToString(),
          ClaimValueTypes.Integer64),
        new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString(), ClaimValueTypes.Integer, _jwtSettings.Issuer),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, ((int) role).ToString(), ClaimValueTypes.String,
          _jwtSettings.Issuer),
        new Claim("Code", code, ClaimValueTypes.String,
          _jwtSettings.Issuer)
      };
      return new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType,
        ClaimsIdentity.DefaultRoleClaimType);
    }

    private static void ThrowIfInvalidOptions(EmailJwtSettings options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidFor <= TimeSpan.Zero)
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(EmailJwtSettings.ValidFor));

      if (options.SigningCredentials == null) throw new ArgumentNullException(nameof(EmailJwtSettings.SigningCredentials));

      if (options.JtiGenerator == null) throw new ArgumentNullException(nameof(EmailJwtSettings.JtiGenerator));
    }

    public bool IsTokenValid(string token)
    {
      //new JwtSecurityTokenHandler().ValidateToken(token, _jwtSettings.TokenValidationParameters,out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);
      //if (validatedToken == null) return false;

      var code = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First(x => x.Type == "Code").Value;
      return !code.IsNullOrEmpty() && code == _jwtSettings.Code;
    }

    public string EnDecodeCode(int userId)
    {
      var claimsIdentity =  GenerateClaimsIdentity(userId, RoleType.User, "").GetAwaiter().GetResult();
      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claimsIdentity.Claims,
        _jwtSettings.NotBefore, DateTime.Now.AddDays(5), _jwtSettings.SigningCredentials);

      return  new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public JwtSecurityToken DecodeCode(string token)
    {
      return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
  }
}
