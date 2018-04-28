using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthorizationServer.Models;
using AuthorizationServer.Repositories;
using AuthorizationServer.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AuthorizationServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Route("/token")]
    public class AccountController : Controller
    {
        //some config in the appsettings.json
        private IOptions<AuthOptions> _settings;

        //repository to handler the sqlite database
        private ITokenRepository _dbToken;

        private IMemoryCache _cache;

        private TwoButtonsContext _db;

        public AccountController(IOptions<AuthOptions> settings, TwoButtonsContext context, ITokenRepository repo, IMemoryCache memoryCache)
        {
            _settings = settings;
            _dbToken = repo;
            _cache = memoryCache;
            _db = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel loginData)
        {
            var login = loginData.login.Trim();
            var password = loginData.password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || !LoginWrapper.TryCheckValidLogin(_db, login, out var isValid) || !isValid)
                return BadRequest("Invalid username or password.");


            switch (loginData.grant_type)
            {
                case "password":
                    return Ok(AccessToken(loginData));
                case "refresh_token":
                    return Ok(RefreshToken(loginData));
                default:
                    return BadRequest();
            }


        }

        // GET api/checkValidLogin/
        [HttpPost("checkValidLogin")]
        public IActionResult CheckValidLogin(string login)
        {
            if (LoginWrapper.TryCheckValidLogin(_db, login, out var isValid))
                return Ok(isValid);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        public IActionResult Logout()
        {
         //   _dbToken.ExpireToken
        }


       

  
        private IActionResult AccessToken(LoginViewModel loginData)
        {
                                       

            if (!LoginWrapper.TryGetIdentification(_db, loginData.login, loginData.password, out var userId) || userId == -1)
                

           
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            var token = new Token
            {
                UserId = userId.ToString(),
                RefreshToken = refreshToken,
                Id = Guid.NewGuid().ToString(),
                IsStop = 0
            };

            //store the refresh_token 
            if (_dbToken.AddToken(token))
            {
                return Ok(GetJwt(userId.ToString(), refreshToken));
            }
            
              return BadRequest("Can not add token to database");
            
        }

        private IActionResult RefreshToken(LoginViewModel loginData)
        {
            var token = _dbToken.GetToken(loginData.refresh_token, loginData.user_id);

            if (token == null)
            {
                return BadRequest("Can not refresh token");
            }

            if (token.IsStop == 1)
            {
                return BadRequest("Refresh token has expired");
            }

            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            token.IsStop = 1;
            //expire the old refresh_token and add a new refresh_token
            var updateFlag = _dbToken.ExpireToken(token);

            var addFlag = _dbToken.AddToken(new Token
            {
                UserId = loginData.UserId,
                RefreshToken = refreshToken,
                Id = Guid.NewGuid().ToString(),
                IsStop = 0
            });

            if (updateFlag && addFlag)
            {
                return Ok(GetJwt(loginData.UserId, refreshToken));
            }

            return BadRequest("Can not expire token or a new token");
        
        }

        private string GetJwt(string clientId, string refreshToken, string role)
        {
            var now = DateTime.UtcNow;

            //var claims = new Claim[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, clientId),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            //};

            var indentity = GetIdentity(role);

            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Issuer,
                audience: _settings.Value.Audience,
                claims: indentity.Claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_settings.Value.Lifetime)),
                signingCredentials: GetSigningCredentials());
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(_settings.Value.Lifetime).TotalSeconds,
                refresh_token = refreshToken,
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private ClaimsIdentity GetIdentity(int userId, string login, string role)
        {

            var claims = new List<Claim>
                {
                    new Claim("UserId", userId.ToString(), ClaimValueTypes.Integer32, _settings.Value.Issuer),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim("Role", role,ClaimValueTypes.String, _settings.Value.Issuer)
                };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var symmetricKeyAsBase64 = _settings.Value.Key;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
