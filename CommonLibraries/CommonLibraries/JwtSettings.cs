using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries
{
  public class JwtSettings
  {

    /// <summary>
    ///   4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    ///   4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    ///   4.1.3.  "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    ///   4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after
    ///   which the JWT MUST NOT be accepted for processing.
    /// </summary>
    public DateTime ExpirationAccessToken => IssuedAt.Add(ValidForAccessToken);

    /// <summary>
    ///   4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after
    ///   which the JWT MUST NOT be accepted for processing.
    /// </summary>
    public DateTime ExpirationRefreshToken => IssuedAt.Add(ValidForRefreshToken);

    /// <summary>
    ///   4.1.5.  "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be
    ///   accepted for processing.
    /// </summary>
    public DateTime NotBefore => DateTime.UtcNow;

    /// <summary>
    ///   4.1.6.  "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
    /// </summary>
    public DateTime IssuedAt => DateTime.UtcNow;

    /// <summary>
    ///   Set the timespan the access token will be valid for (default is 120 min)
    /// </summary>
    public TimeSpan ValidForAccessToken { get; set; } = TimeSpan.FromMinutes(120);

    /// <summary>
    ///   Set the timespan the refresh token will be valid for (default is 60 days)
    /// </summary>
    public TimeSpan ValidForRefreshToken { get; set; } = TimeSpan.FromMinutes(60 * 24 * 60);

    /// <summary>
    ///   "jti" (JWT ID) Claim (default ID is a GUID)
    /// </summary>
    public Func<Task<string>> JtiGenerator =>
      () => Task.FromResult(Guid.NewGuid().ToString());

    /// <summary>
    /// The signing key to use when generating tokens.
    /// </summary>
    public SigningCredentials SigningCredentials { get; set; }

    public static SymmetricSecurityKey CreateSecurityKey(string secretKey)
    {
      var keyByteArray = Encoding.ASCII.GetBytes(secretKey);
      return new SymmetricSecurityKey(keyByteArray);
    }

    public static TokenValidationParameters CreateTokenValidationParameters(string issuer, string audience, SymmetricSecurityKey symmetricSecurityKey)
     
    {
      return  new TokenValidationParameters { 
      // укзывает, будет ли валидироваться издатель при валидации токена
      ValidateIssuer = true,
      // строка, представляющая издателя
      ValidIssuer = issuer,

      // будет ли валидироваться потребитель токена
      ValidateAudience = true,
      // установка потребителя токена
      ValidAudience = audience,


      // установка ключа безопасности
      IssuerSigningKey = symmetricSecurityKey,
      // валидация ключа безопасности
      ValidateIssuerSigningKey = true,

      // будет ли валидироваться время существования
      ValidateLifetime = true,
      // RequireExpirationTime = false,
      ClockSkew = TimeSpan.Zero
    };
  }
}
}