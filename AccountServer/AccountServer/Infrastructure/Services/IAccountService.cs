using System;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using Microsoft.AspNetCore.Http;

namespace AccountServer.Infrastructure.Services
{
  public interface IAccountService : IDisposable
  {
    Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType);
    Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId);
    Task<bool> UpdateAvatarViaFile(int userId, AvatarSizeType avatarSize, IFormFile file);
    Task<bool> UpdateAvatarViaLink(int userId, AvatarSizeType avatarSize, string newAvatarUrl);
    Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user);
  }
}