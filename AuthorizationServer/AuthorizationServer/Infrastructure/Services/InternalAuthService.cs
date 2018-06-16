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
using AuthorizationServer.Infrastructure.EmailJwt;
using AuthorizationServer.Infrastructure.Jwt;
using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using CommonLibraries.Helpers;
using CommonLibraries.Response;

namespace AuthorizationServer.Infrastructure.Services
{
  public class InternalAuthService : IInternalAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IJwtService _jwtService;
    private readonly IEmailJwtService _emailJwtService;
    public InternalAuthService(IJwtService jwtService, IEmailJwtService emailJwtService, AuthorizationUnitOfWork db)
    {
      _db = db;
      _jwtService = jwtService;
      _emailJwtService = emailJwtService;
    }

    public async Task<Token> RegisterAsync(UserRegistrationViewModel user)
    {
      var isExistByPhone = !user.Phone.IsNullOrEmpty() && await _db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = !user.Email.IsNullOrEmpty() && await  _db.Users.IsUserExistByEmailAsync(user.Email);

   if (isExistByEmail || isExistByPhone)
        throw new Exception("You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserDb
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetHashString(),
        RegistrationDate = DateTime.UtcNow
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
        LargeAvatarLink = MediaServerHelper.StandardAvatar(AvatarSizeType.LargeAvatar),
        SmallAvatarLink = MediaServerHelper.StandardAvatar(AvatarSizeType.SmallAvatar)
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your indformation. Please, tell us about it.");
      }

      MonitoringServerHelper.AddUrlMonitoring(userDb.UserId);


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

    public async Task<UserDto> GetUserByCredentils(LoginViewModel credentials)
    {
      var user = new UserDto { UserId = 0, RoleType = RoleType.Guest };

      if (credentials.GrantType == GrantType.Guest) return user;

      switch (credentials.GrantType)
      {
        case GrantType.Guest:
          break;
        case GrantType.Phone:
          user = await _db.Users.GetUserByInernalPhoneAndPasswordAsync(credentials.Phone, credentials.Password);
          if (user == null)
          {
            throw new NotFoundException("Phone and (or) password is incorrect");
          }
          break;
        case GrantType.Email:
          user = await _db.Users.GetUserByInternalEmailAndPasswordAsync(credentials.Email, credentials.Password);
          if (user == null)
          {
            throw new NotFoundException("Email and (or) password is incorrect");
          }
          break;
      }

      user.RoleType = await _db.Users.GetUserRoleAsync(user.UserId);
      return user;
    }

    public async Task<bool> ConfirmEmail(int userId, string token)
    {
      if (!_emailJwtService.IsTokenValid(token)) return false;
      var decodedToken = _emailJwtService.DecodeCode(token);

      var userTokenId = int.Parse(decodedToken.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "0");
      return userId == userTokenId && userId != 0 && await _db.Users.ConfirmEmail(userId);
    }

    public async Task<string> GetConfirmedEmailToken(int userId, RoleType role)
    {
      return await _emailJwtService.GenerateJwtAsync(userId, role);
    }

    public void Dispose()
    {
      _db.Dispose();
    }
  }
}
