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
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AuthorizationServer.Infrastructure.Services
{
  public class InternalAuthService : IInternalAuthService
  {
    private  AuthorizationUnitOfWork Db { get; }
    private  ConnectionsHub Hub { get; }
    private  IEmailJwtService EmailJwtService { get; }
    private  IJwtService JwtService { get; }
    private  IStringLocalizer<InternalAuthService> Localizer { get; }
    private  ILogger<ExternalAuthService> Logger { get; }

    public InternalAuthService(ConnectionsHub hub, IJwtService jwtService, IEmailJwtService emailJwtService, AuthorizationUnitOfWork db, IStringLocalizer<InternalAuthService> localizer, ILogger<ExternalAuthService> logger)
    {
      Db = db;
      Hub = hub;
      JwtService = jwtService;
      EmailJwtService = emailJwtService;
      Localizer = localizer;
      Logger = logger;
    }

    public async Task<Token> RegisterAsync(UserRegistrationViewModel user)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(RegisterAsync)}.Start");
      var isExistByPhone = !user.Phone.IsNullOrEmpty() && await Db.Users.IsUserExistByPhoneAsync(user.Phone);
      var isExistByEmail = !user.Email.IsNullOrEmpty() && await Db.Users.IsUserExistByEmailAsync(user.Email);

      if (isExistByEmail || isExistByPhone) throw new Exception("You have already registered.");

      const RoleType role = RoleType.User;

      var userDb = new UserEntity
      {
        Email = user.Email,
        PhoneNumber = user.Phone,
        RoleType = role,
        PasswordHash = user.Password.GetMd5HashString(),
        RegistrationDate = DateTime.UtcNow,
      };
      var isAdded = await Db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      string firstName;
      string lastName;

      var spacePosition = user.Login.IndexOf(' ');
      if (spacePosition == -1)
      {
        firstName = user.Login.Substring(0, user.Login.Length);
        lastName = "";
      }
      else
      {
        firstName = user.Login.Substring(0, spacePosition);
        lastName = user.Login.Substring(spacePosition + 1).Trim();
      }
      var userInfo = new UserInfoQuery
      {
        UserId = userDb.UserId,
        FirstName = firstName,
        LastName = lastName,
        BirthDate = user.BirthDate,
        SexType = user.SexType,
        City = user.City,
        Description = user.Description ?? "",
        OriginalAvatarUrl = (await Hub.Media.GetStandardAvatarUrls(AvatarSizeType.Original)).FirstOrDefault() ?? ""
      };

      if (!await Db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await Db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your information. Please, tell us about it.");
      }

      //await Hub.Monitoring.AddUrlMonitoring(userDb.UserId);

      var jwtToken = await JwtService.GenerateJwtAsync(userDb.UserId, role);

      var token = new TokenEntity
      {
        UserId = userDb.UserId,
        ExpiresIn = jwtToken.ExpiresIn,
        RefreshToken = jwtToken.RefreshToken
      };

      // SendConfirmedEmail(userDb.UserId, role, userDb.Email);

      if (!await Db.Tokens.AddTokenAsync(token))
        throw new Exception("Can not add token to database. You entered just as a guest.");
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(RegisterAsync)}.End");
      return jwtToken;
    }

    public async Task<UserDto> GetUserByPhone(string phone, string password)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(GetUserByPhone)}.Start");
      var user = await Db.Users.GetUserByInernalPhoneAndPasswordAsync(phone, password);
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(GetUserByPhone)}.End");
      return user;
    }

    public async Task<UserDto> GetUserByEmail(string email, string password)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(GetUserByEmail)}.Start");
      var user = await Db.Users.GetUserByInternalEmailAndPasswordAsync(email, password);
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(GetUserByEmail)}.End");
      return user;
    }

    public bool IsTokenValid(string token)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(IsTokenValid)}.Start");
      var result = EmailJwtService.IsTokenValid(token);
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(IsTokenValid)}.End");
      return result;
    }

    public async Task<bool> TryConfirmEmail(int userId, string token)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(TryConfirmEmail)}.Start");
      var decodedToken = EmailJwtService.DecodeCode(token);

      var userTokenId = int.Parse(decodedToken.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)
                                    ?.Value ?? "0");
      var result = userId == userTokenId && userId != 0 && await Db.Users.ConfirmEmail(userId);
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(TryConfirmEmail)}.End");
      return result;
    }

    public async Task<bool> ResetPassword(string token, string email, string passwordHash)
    {
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(ResetPassword)}.Start");
      var user = await Db.Users.GetUserByInternalEmail(email);
      if (user == null || !user.EmailConfirmed)
        throw new NotFoundException("We can not find this email or email is not confirmed");
      var result = await Db.Users.ResetPasswordAsync(email, passwordHash);
      Logger.LogInformation($"{nameof(InternalAuthService)}.{nameof(ResetPassword)}.End");
      return result;
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
      Db.Dispose();
    }
  }
}