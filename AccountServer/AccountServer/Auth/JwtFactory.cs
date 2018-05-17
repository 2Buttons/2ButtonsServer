using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AccountServer.Helpers;
using AccountServer.Models;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Options;

namespace AccountServer.Auth
{
  public class JwtFactory : IJwtFactory
  {
    private readonly JwtIssuerOptions _jwtOptions;

    public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
    {
      _jwtOptions = jwtOptions.Value;
      ThrowIfInvalidOptions(_jwtOptions);
    }

    public async Task<string> GenerateEncodedAccessToken(int userId, RoleType role)
    {
      var claimsIdentity = await GenerateClaimsIdentity(userId, role);

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
        issuer: _jwtOptions.Issuer,
        audience: _jwtOptions.Audience,
        claims: claimsIdentity.Claims,
        notBefore: _jwtOptions.NotBefore,
        expires: _jwtOptions.ExpirationAccessToken,
        signingCredentials: _jwtOptions.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }



    public async Task<string> GenerateEncodedRefreshToken(int userId, RoleType role)
    {
      var claimsIdentity = await GenerateClaimsIdentity(userId, role);

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
        issuer: _jwtOptions.Issuer,
        audience: _jwtOptions.Audience,
        claims: claimsIdentity.Claims,
        notBefore: _jwtOptions.NotBefore,
        expires: _jwtOptions.ExpirationRefreshToken,
        signingCredentials: _jwtOptions.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }
        public async Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
        new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString(), ClaimValueTypes.Integer,  _jwtOptions.Issuer),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString(),ClaimValueTypes.String, _jwtOptions.Issuer)
      };
      return new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
    private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidForAccessToken <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidForAccessToken));
      }

      if (options.ValidForRefreshToken <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidForRefreshToken));
      }

      if (options.SigningCredentials == null)
      {
        throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
      }

      if (options.JtiGenerator == null)
      {
        throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
      }
    }
  }
}
