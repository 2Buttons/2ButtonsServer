using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonLibraries.SocialNetworks.Vk
{
  public interface IVkService
  {
    Task<VkAppAccessToken> GetAccessTokenAsync(string vkCode, bool isTest = false);
    Task<List<VkFriendData>> GetUserFriendsAsync(long vkId);
    Task<NormalizedSocialUserData> GetUserInfoAsync(string code, bool isTest = false);
    Task<NormalizedSocialUserData> GetUserInfoAsync(long externalUserId, string email, string externalToken, long expiresIn);
  }
}