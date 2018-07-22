using System;
using System.Linq;
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
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Localization;

namespace AuthorizationServer.Infrastructure.Services
{
  public class InternalAuthService : IInternalAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly ConnectionsHub _hub;
    private readonly IEmailJwtService _emailJwtService;
    private readonly IJwtService _jwtService;
    private readonly IStringLocalizer<InternalAuthService> _localizer;

    public InternalAuthService(ConnectionsHub hub, IJwtService jwtService, IEmailJwtService emailJwtService, AuthorizationUnitOfWork db, IStringLocalizer<InternalAuthService> localizer)
    {
      _db = db;
      _hub = hub;
      _jwtService = jwtService;
      _emailJwtService = emailJwtService;
      _localizer = localizer;
    }

    public async Task<Token> RegisterAsync(UserRegistrationViewModel user)
    {
      var isExistByPhone = !user.Phone.IsNullOrEmpty() && await _db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = !user.Email.IsNullOrEmpty() && await _db.Users.IsUserExistByEmailAsync(user.Email);

      if (isExistByEmail || isExistByPhone) throw new Exception("You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserDb
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetMd5HashString(),
        RegistrationDate = DateTime.UtcNow
      };
      var isAdded = await _db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        SexType = user.SexType,
        City = user.City,
        Description = user.Description,
        OriginalAvatarUrl =( await _hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault()
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your information. Please, tell us about it.");
      }

      await _hub.Monitoring.AddUrlMonitoring(userDb.UserId);

      var jwtToken = await _jwtService.GenerateJwtAsync(userDb.UserId, role);

      var token = new TokenDb
      {
        UserId = userDb.UserId,
        ExpiresIn = jwtToken.ExpiresIn,
        RefreshToken = jwtToken.RefreshToken
      };

    // SendConfirmedEmail(userDb.UserId, role, userDb.Email);

      if (!await _db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      return jwtToken;
    }

    public async Task<UserDto> GetUserByCredentils(LoginViewModel credentials)
    {
      var user = new UserDto {UserId = 0, RoleType = RoleType.Guest};

      if (credentials.GrantType == GrantType.Guest) return user;

      switch (credentials.GrantType)
      {
        case GrantType.Guest: break;
        case GrantType.Phone:
          user = await _db.Users.GetUserByInernalPhoneAndPasswordAsync(credentials.Phone, credentials.Password);
          if (user == null) throw new NotFoundException("Phone and (or) password is incorrect");
          break;
        case GrantType.Email:
          user = await _db.Users.GetUserByInternalEmailAndPasswordAsync(credentials.Email, credentials.Password);
          if (user == null) throw new NotFoundException("Email and (or) password is incorrect");
          break;
      }

      user.RoleType = await _db.Users.GetUserRoleAsync(user.UserId);
      return user;
    }

    public bool IsTokenValid(string token)
    {
      return _emailJwtService.IsTokenValid(token);
    }

    public async Task<bool> TryConfirmEmail(int userId, string token)
    {
      var decodedToken = _emailJwtService.DecodeCode(token);

      var userTokenId = int.Parse(decodedToken.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)
                                    ?.Value ?? "0");
      return userId == userTokenId && userId != 0 && await _db.Users.ConfirmEmail(userId);
    }

    public async Task<bool> ResetPassword(string token, string email, string passwordHash)
    {
      var user = await _db.Users.GetUserByInternalEmail(email);
      if (user == null || !user.EmailConfirmed)
        throw new NotFoundException("We can not find this email or email is not confirmed");
      return await _db.Users.ResetPasswordAsync(email, passwordHash);
    }

    //public async Task<bool> SendConfirmation(int userId)
    //{
    //  var user = await _db.Users.GetUserByUserId(userId);
    //  if (user == null) return false;
    //  await SendConfirmedEmail(user.UserId, user.RoleType, user.Email);
    //  return true;
    //}

    //public async Task<bool> SendForgotPassword(string email)
    //{
    //  var user = await _db.Users.GetUserByInternalEmail(email);
    //  if (user == null || !user.EmailConfirmed)
    //    throw new NotFoundException("We can not find this email or email is not confirmed");
    //  await SendForgotPasswordConfirmation(user.UserId, user.RoleType, user.Email);
    //  return true;
    //}

    //public async Task<bool> SendCongratilationsThatEmailConfirmed(int userId)
    //{
    //  var user = await _db.Users.GetUserByUserId(userId);
    //  if (user == null || !user.EmailConfirmed)
    //    throw new NotFoundException("We can not find this email or email is not confirmed");
    //  await SendCongratilationsThatEmailConfirmed(user.UserId, user.RoleType, user.Email);
    //  return true;
    //}

    //private async Task SendCongratilationsThatEmailConfirmed(int userId, RoleType role, string email)
    //{
    //  var emailToken = await _emailJwtService.GenerateJwtAsync(userId, role);
    //  var callbackUrl = $"https://2buttons.ru";

    //  new EmailSender().SednNoReply(email, _localizer["SubjectEmailConfirmed"],
    //    _localizer["BodyEmailConfirmed"] + $": <a href='{callbackUrl}'>" + "2 Buttons" + "</a> " + _localizer["IfUrlDoesNotWork"] + $": {callbackUrl}.");
    //}

    //public async Task<bool> SendResetPassword(string email)
    //{
    //  var user = await _db.Users.GetUserByInternalEmail(email);
    //  if (user == null || !user.EmailConfirmed)
    //    throw new NotFoundException("We can not find this email or email is not confirmed");
    //  await SendResetPasswordConfirmation(user.UserId, user.RoleType, user.Email);
    //  return true;
    //}

    //private async Task SendResetPasswordConfirmation(int userId, RoleType role, string email)
    //{
    //  var emailToken = await _emailJwtService.GenerateJwtAsync(userId, role);
    //  var callbackUrl = $"http://localhost:6001/forgotPassword.html?token={emailToken}";

    //  new EmailSender().SednNoReply(email, _localizer["SubjectResetPassword"],
    //    _localizer["BodyResetPassword"] + $": <a href='{callbackUrl}'>" + _localizer["ResetPasswordUrl"] + "</a> " + _localizer["IfUrlDoesNotWork"] + $": {callbackUrl}.");
    //}

    //private async Task SendForgotPasswordConfirmation(int userId, RoleType role, string email)
    //{
    //  var emailToken = await _emailJwtService.GenerateJwtAsync(userId, role);
    //  var callbackUrl = $"http://localhost:6001/forgotPassword.html?token={emailToken}";

    //  new EmailSender().SednNoReply(email, _localizer["SubjectForgotPassword"],
    //    _localizer["BodyForgotPassword"] + $": <a href='{callbackUrl}'>" + _localizer["ForgotPasswordUrl"] + "</a> " + _localizer["IfUrlDoesNotWork"] + $": {callbackUrl}.");
    //}

    //private async Task SendConfirmedEmail(int userId, RoleType role, string email)
    //{
    //  var emailToken = await _emailJwtService.GenerateJwtAsync(userId, role);
    //  var callbackUrl = $"http://localhost:6001/auth/confirm/email?userId={userId}&token={emailToken}";
    //  new EmailSender().SednNoReply(email, _localizer["SubjectConfirmEmail"],
    //    _localizer["BodyConfirmEmail"] + $": <a href='{callbackUrl}'>" + _localizer["ConfirmEmailUrl"] + "</a> " + _localizer["IfUrlDoesNotWork"] + $": {callbackUrl}.");
    //}

    public void Dispose()
    {
      _db.Dispose();
    }
  }
}