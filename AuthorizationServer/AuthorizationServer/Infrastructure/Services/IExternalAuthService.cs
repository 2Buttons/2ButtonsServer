using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Main.Entities;
using CommonLibraries;
using CommonLibraries.SocialNetworks;

namespace AuthorizationServer.Infrastructure.Services
{
  public interface IExternalAuthService : IDisposable
  {
    Task<bool> AddUserSocialAsync(int internalId, SocialType socialType, NormalizedSocialUserData socialUserData);
    Task<UserDto> GetUserViaExternalSocialNet(string code, SocialType socialType);
  }
}