//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AccountServer.Helpers;
//using AccountServer.ViewModels.InputParameters;
//using CommonLibraries;
//using Microsoft.AspNetCore.Mvc;
//using TwoButtonsAccountDatabase.Entities;

//namespace AccountServer.Auth
//{
//    public class TokenManager
//    {

//      private async Task<IActionResult> AccessToken(CredentialsViewModel credentials)
//      {
//        var nowTime = DateTime.UtcNow;
//        var expiresAccessTokenInTime = 5;

//        var userId = 0;
//        var role = RoleType.Guest;

//        if (credentials.GrantType == GrantType.NoGrantType)
//          role = await _accountDb.Users.GetUserRoleAsync(userId);

//        ClientDb client = null;
//        if (!string.IsNullOrEmpty(credentials.SecretKey))
//          client = await _accountDb.Clients.FindClientAsync(credentials.ClientId, credentials.SecretKey);

//        if (client == null)
//        {
//          int expiresRefreshTokenInTime;
//          switch (role)
//          {
//            case RoleType.Guest:
//              expiresRefreshTokenInTime = 60 * 24 * 7 * 4; // 1 month
//              break;
//            case RoleType.User when credentials.IsRememberMe:
//            case RoleType.Moderator when credentials.IsRememberMe:
//            case RoleType.Admin when credentials.IsRememberMe:
//              expiresRefreshTokenInTime = 60 * 24 * 7 * 2; //2 weeks
//              break;
//            default:
//              expiresRefreshTokenInTime = 120; //2 hours
//              break;
//          }

//          client = new ClientDb
//          {
//            Secret = Guid.NewGuid().ToString(),
//            IsActive = true,
//            RefreshTokenLifeTime = expiresRefreshTokenInTime
//          };
//          await _accountDb.Clients.AddClientAsync(client);
//        }
//        if (!client.IsActive)
//        {
//          client.IsActive = true;
//          await _accountDb.Clients.UpdateClientAsync(client);
//        }

//        var refreshToken = Guid.NewGuid().ToString();

//        var token = new TokenDb
//        {
//          UserId = userId,
//          ClientId = client.ClientId,
//          IssuedUtc = nowTime,
//          ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
//          RefreshToken = refreshToken
//        };

//        if (!await _accountDb.Tokens.AddTokenAsync(token))
//          return BadRequest("Can not add token to database. You entered just as a guest.");


//        _jwtOptions.ValidFor = TimeSpan.FromMinutes(expiresAccessTokenInTime);
//        return Ok(Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role,
//          _jwtOptions));
//      }

//      private async Task<IActionResult> RefreshToken(CredentialsViewModel credentials)
//      {
//        var nowTime = DateTime.UtcNow;

//        if (string.IsNullOrEmpty(credentials.RefreshToken) || string.IsNullOrEmpty(credentials.SecretKey))
//          return BadRequest("RefreshToken or SecretKey is null or empty. Please, send again.");

//        var client = await _accountDb.Clients.FindClientAsync(credentials.ClientId, credentials.SecretKey);
//        if (client == null || !client.IsActive)
//          return BadRequest(
//            "Sorry, you have not loge in yet or your connection with authorization server is expired. Plese, get access token again");

//        var oldToken = await _accountDb.Tokens.FindTokenAsync(credentials.ClientId, credentials.RefreshToken);
//        if (oldToken == null)
//          return BadRequest("Sorry, we can not find your \"refresh token\". Plese, get access token again.");

//        var role = RoleType.Guest;
//        if (oldToken.UserId != 0)
//          role = await _accountDb.Users.GetUserRoleAsync(oldToken.UserId);

//        var refreshToken = Guid.NewGuid().ToString();

//        var token = new TokenDb
//        {
//          UserId = oldToken.UserId,
//          ClientId = client.ClientId,
//          IssuedUtc = nowTime,
//          ExpiresUtc = nowTime.AddMinutes(client.RefreshTokenLifeTime),
//          RefreshToken = refreshToken
//        };

//        var isDeleted = await _accountDb.Tokens.RemoveTokenAsync(oldToken);
//        var isAdded = await _accountDb.Tokens.AddTokenAsync(token);

//        if (!isDeleted || !isAdded)
//          return BadRequest("Can not add token to database. Plese, get access token again or enter like a guest");

//        _jwtOptions.ValidFor = TimeSpan.FromMinutes(client.RefreshTokenLifeTime);
//        return Ok(Tokens.GenerateJwtAsync(_jwtFactory, client.ClientId, client.Secret, token.RefreshToken, token.UserId, role,
//          _jwtOptions));
//      }
//  }
//}
