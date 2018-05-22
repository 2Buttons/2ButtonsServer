using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationServer.Models;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Options;

namespace AuthorizationServer.Services
{
  public class JwtService : IJwtService
  {
    private readonly JwtSettings _jwtSettings;
    public JwtService(IOptions<JwtSettings> jwtOptions)
    {
      _jwtSettings = jwtOptions.Value;
      ThrowIfInvalidOptions(_jwtSettings);
    }


    public async Task<Token> GenerateJwtAsync(int userId, RoleType role)
    {
      var response = new Token
      {
        UserId = userId,
        RoleType = role,
        AccessToken = await GenerateEncodedAccessTokenAsync(userId, role),
        ExpiresIn = _jwtSettings.ExpirationAccessToken.ToUnixEpochDate(),
        RefreshToken = await GenerateEncodedRefreshTokenAsync(userId, role)
      };

      return response;
    }

    private async Task<string> GenerateEncodedAccessTokenAsync(int userId, RoleType role)
    {
      var claimsIdentity = await GenerateClaimsIdentity(userId, role);

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claimsIdentity.Claims,
        notBefore: _jwtSettings.NotBefore,
        expires: _jwtSettings.ExpirationAccessToken,
        signingCredentials: _jwtSettings.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }


    private async Task<string> GenerateEncodedRefreshTokenAsync(int userId, RoleType role)
    {
      var claimsIdentity = await GenerateClaimsIdentity(userId, role);

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claimsIdentity.Claims,
        notBefore: _jwtSettings.NotBefore,
        expires: _jwtSettings.ExpirationRefreshToken,
        signingCredentials: _jwtSettings.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }
    public async Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtSettings.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, _jwtSettings.IssuedAt.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
        new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString(), ClaimValueTypes.Integer,  _jwtSettings.Issuer),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, ((int)role).ToString(),ClaimValueTypes.String, _jwtSettings.Issuer)
      };
      return new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
    private static void ThrowIfInvalidOptions(JwtSettings options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidForAccessToken <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtSettings.ValidForAccessToken));
      }

      if (options.ValidForRefreshToken <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtSettings.ValidForRefreshToken));
      }

      if (options.SigningCredentials == null)
      {
        throw new ArgumentNullException(nameof(JwtSettings.SigningCredentials));
      }

      if (options.JtiGenerator == null)
      {
        throw new ArgumentNullException(nameof(JwtSettings.JtiGenerator));
      }
    }
  }
}
