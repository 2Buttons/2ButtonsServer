using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonLibraries.SocialNetworks.Vk
{
  public interface IVkService
  {
    Task<VkAppAccessToken> GetAccessTokenAsync(string vkCode);
    Task<List<VkFriendData>> GetUserFriendsAsync(long vkId);
    Task<NormalizeSocialUserData> GetUserInfoAsync(string code);
  }
}