using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Main.Entities;
using CommonLibraries;
using CommonLibraries.SocialNetworks;
using CommonTypes;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IExternalAuthService : IDisposable
  {
    Task<bool> AddUserSocialAsync(int internalId, SocialType socialType, NormalizedSocialUserData socialUserData);
    Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType, bool isTest = false);
    Task<UserDto> GetUserViaExternalSocialNet(long externalUserId, string email, string externalToken, long expiresIn, SocialType socialType);
  }
}