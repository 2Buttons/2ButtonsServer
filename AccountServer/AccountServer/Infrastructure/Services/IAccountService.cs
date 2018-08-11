using System;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using Microsoft.AspNetCore.Http;

namespace AccountServer.Infrastructure.Services
{
  public interface IAccountService
  {
    Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType);
    Task<(string city, DateTime birthdate)> GetCityAndBirthdate(int userId);
    Task<string> GetUserAvatar(int userId, AvatarSizeType avatarSizeType);
    Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId);
    Task<(bool isUpdated, string url)> UpdateAvatarViaFile(int userId, AvatarType avatarType, AvatarSizeType avatarSizeType, IFormFile file);
    Task<(bool isUpdated, string url)> UpdateAvatarViaUrl(int userId, AvatarType avatarType, AvatarSizeType avatarSizeType, string newAvatarUrl);
    Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user);
  }
}