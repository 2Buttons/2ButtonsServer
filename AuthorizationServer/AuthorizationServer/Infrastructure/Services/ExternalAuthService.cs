﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Newtonsoft.Json;

namespace AuthorizationServer.Infrastructure.Services
{
  public class ExternalAuthService : IExternalAuthService
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IFbService _fbService;
    private readonly IVkService _vkService;

    public ExternalAuthService(AuthorizationUnitOfWork db, IVkService vkService, IFbService fbService)
    {
      _db = db;
      _vkService = vkService;
      _fbService = fbService;
    }

    public void Dispose()
    {
      _db.Dispose();
    }

    public async Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType)
    {
      NormalizedSocialUserData socialUserData;
      switch (socialType)
      {
        case SocialType.Facebook:
          socialUserData = await _fbService.GetUserInfoAsync(code);
          break;
        case SocialType.Vk:
          socialUserData = await _vkService.GetUserInfoAsync(code);
          break;
        case SocialType.Twiter:
        case SocialType.GooglePlus:
        case SocialType.Telegram:
        case SocialType.Badoo:
        case SocialType.Nothing:
        default: throw new Exception($"We do not support loging in via {socialType}.");
      }

      var user = await _db.Socials.FindUserByExternalUserIdAsync(socialUserData.ExternalId, socialType);
      if (user != null) return user;

      user = await FindUserByExternalEmail(socialUserData.ExternalEmail);
      if (user == null) return await RegisterViaExternalSocial(socialUserData, socialType);

      if(!await AddExternalEnter(user.UserId, socialType, socialUserData))
        throw new Exception(
          $"We can not your information about your {socialType} account in our system. Please, log in via email or phone or change social network to another.");
      return user;
    }

    private async Task<bool> AddExternalEnter(int userId, SocialType socialType, NormalizedSocialUserData socialUserData)
    {
      if (!await AddUserSocialAsync(userId, socialType, socialUserData)) return false;
      var userInfo = await _db.UsersInfo.GetUserInfoAsync(userId);
      if (userInfo.SmallAvatarLink.Contains("stand") && !socialUserData.SmallPhotoUrl.IsNullOrEmpty())
      {
        userInfo.SmallAvatarLink = await GetStandardPhotoUrl(AvatarSizeType.UserSmallAvatarPhoto);
      }
      if (userInfo.FullAvatarLink.Contains("stand") && !socialUserData.FullPhotoUrl.IsNullOrEmpty())
      {
        userInfo.FullAvatarLink = await GetStandardPhotoUrl(AvatarSizeType.UserFullAvatarPhoto);
      }

      await _db.UsersInfo.UpdateUserInfoAsync(userInfo);
      return true;
    }

    private async Task<UserDto> RegisterViaExternalSocial(NormalizedSocialUserData user, SocialType socialType)
    {
      const RoleType role = RoleType.User;

      var userDb = new UserDb { Email = user.ExternalEmail, RoleType = role, RegistrationDate = DateTime.UtcNow };
      var isAdded = await _db.Users.AddUserAsync(userDb);
      if (!isAdded || userDb.UserId == 0) throw new Exception("We are not able to add you. Please, tell us about it.");

      var links = (smallLink : string.Empty, fullLink: string.Empty);
      try
      {
        links = await UploadAvatars(userDb.UserId, user.SmallPhotoUrl, user.FullPhotoUrl);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      var userInfo = new UserInfoDb
      {
        UserId = userDb.UserId,
        Login = user.Login,
        BirthDate = user.BirthDate,
        Sex = user.Sex,
        City = user.City,
        FullAvatarLink = links.fullLink == string.Empty ? null : links.fullLink,
        SmallAvatarLink = links.smallLink == string.Empty ? null : links.smallLink
      };

      if (!await _db.UsersInfo.AddUserInfoAsync(userInfo))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your indformation. Please, tell us about it.");
      }

     
      if (!await AddUserSocialAsync(userDb.UserId, socialType, user))
      {
        await _db.Users.RemoveUserAsync(userDb.UserId);
        throw new Exception("We are not able to add your social information. Please, tell us about it.");
      }

      
      return new UserDto { UserId = userDb.UserId, RoleType = userDb.RoleType };
    }

    public async Task<bool> AddUserSocialAsync(int internalId, SocialType socialType,
      NormalizedSocialUserData socialUserData)
    {
      var social = new SocialDb
      {
        InternalId = internalId,
        SocialType = socialType,
        ExternalId = socialUserData.ExternalId,
        Email = socialUserData.ExternalEmail,
        ExternalToken = socialUserData.ExternalToken,
        ExpiresIn = socialUserData.ExpiresIn
      };
      return await _db.Socials.AddUserSocialAsync(social);
    }

    private async Task<UserDto> FindUserByExternalEmail(string externalEmail)
    {
      if (externalEmail.IsNullOrEmpty()) return null;
      var user = await _db.Users.GetUserByInternalEmail(externalEmail);
      if (user != null) return user;
      return await _db.Socials.FindUserByExternalEmaildAsync(externalEmail);
    }

    private async Task<(string smallLink, string fullLink)> UploadAvatars(int userId, string smallPhotoUrl,
      string fullPhotoUrl)
    {
      var jsonSmall = JsonConvert.SerializeObject(new {size = 0, url = smallPhotoUrl });
      var jsonFull = JsonConvert.SerializeObject(new {size = 1, url = fullPhotoUrl });
      var s = await UploadPhoto("http://localhost:6250/upload/avatar/link", jsonSmall);
      var f = await UploadPhoto("http://localhost:6250/upload/avatar/link", jsonFull);

      //await Task.WhenAll(f, s);
      //return (s.Result, f.Result);
      return (s, f);
    }

    private static async Task<string> GetStandardPhotoUrl(AvatarSizeType avatarSize)
    {
      var body = JsonConvert.SerializeObject(new { size = (int)avatarSize });
      var request = WebRequest.Create("http://localhost:6250/standard/avatar/");
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(body);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return reader.ReadToEnd();
      }
    }

    private static async Task<string> UploadPhoto(string url, string requestJson)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(requestJson);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return reader.ReadToEnd();
      }
    }
  }
}