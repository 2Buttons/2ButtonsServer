using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;
using AuthorizationServer.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;

namespace AuthorizationServer.Infrastructure.Services
{
  public class InternalAuthService : IInternalAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IJwtService _jwtService;

    public InternalAuthService(IJwtService jwtService, AuthorizationUnitOfWork db)
    {
      _db = db;
      _jwtService = jwtService;
    }

    public async Task<Token> RegisterAsync(UserRegistrationViewModel user)
    {
      var isExistByPhone = _db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = _db.Users.IsUserExistByPhoneAsync(user.Email);

      await Task.WhenAll(isExistByPhone, isExistByEmail);

      if (isExistByEmail.Result || isExistByPhone.Result)
        throw new Exception("You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserDb
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetHashString()
      };
      var isAdded = await _db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0)
        throw new Exception("We are not able to add you. Please, tell us about it.");

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        Sex = user.SexType,
        City = user.City,
        Description = user.Description,
        FullAvatarLink = user.FullAvatarLink,
        SmallAvatarLink = user.SmallAvatarLink
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your indformation. Please, tell us about it.");
      }


      var jwtToken = await _jwtService.GenerateJwtAsync(userDb.UserId, role);

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ExpiresIn = jwtToken.ExpiresIn,
        RefreshToken = jwtToken.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      return jwtToken;
    }

    public async Task<Token> GetAccessTokenAsync(LoginViewModel credentials)
    {
      var user = new UserDto { UserId = 0 };
      var role = RoleType.Guest;

      if (credentials.GrantType != GrantType.Guest)
      {
        switch (credentials.GrantType)
        {
          case GrantType.Guest:
            break;
          case GrantType.Password:
            user = await _db.Users.GetUserByInernalPhoneAndPasswordAsync(credentials.Phone, credentials.Password);
            if (user == null)
            {             
              throw new Exception("Phone and (or) password is incorrect");
            }
            break;
          case GrantType.Email:
            user = await _db.Users.GetUserByInternalEmailAndPasswordAsync(credentials.Email, credentials.Password);
            if (user == null)
            {
              throw new Exception("Email and (or) password is incorrect");
            }
            break;
        }

        role = await _db.Users.GetUserRoleAsync(user.UserId);

        if (await _db.Tokens.CountTokensForUserAsync(user.UserId) > 10)
        {
          await _db.Tokens.RemoveTokensByUserIdAsync(user.UserId);
          throw new Exception("You logged in at least 10 defferent devices. We are forced to save your data. Now you are logged out of all devices. Please log in again.");
        }
      }

      var result = await _jwtService.GenerateJwtAsync(user.UserId, role);

      var token = new TokenDb
      {
        UserId = user.UserId,
        ExpiresIn = result.ExpiresIn,
        RefreshToken = result.RefreshToken
      };

      if (!await _db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      //var p = new JwtSecurityTokenHandler().ReadJwtToken(result.RefreshToken).ValidTo;

      return result;
    }

    public void Dispose()
    {
      _db.Dispose();
    }
  }
}
