using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountServer.SocialNets
{
    public class VkMethods
    {
      private static readonly HttpClient Client = new HttpClient();

    private readonly string _accessToken;
    public VkMethods(string accessToken)
    {
      _accessToken = accessToken;
    }

    //  public async Task<string> GetAccountInfo()
    //  {
    //  return await Client.GetStringAsync(
    //    $"https://api.vk.com/method/account.getInfo?fields=phone,email&access_token={_accessToken}&v=5.74");
    //  //https://api.vk.com/method/METHOD_NAME?PARAMETERS&access_token=ACCESS_TOKEN&v=V 
    //}

      public async Task<string> GetFriends(int userId, int count=5000, int offset = 0)
      {
        return await Client.GetStringAsync(
          $"https://api.vk.com/method/friends.get?userid={userId}&count={count}&offset={offset}&fields=nickname,first_name,last_name,domain,photo_id&name_case=nom&access_token={_accessToken}&v=5.74");
      }

      public async Task<string> GetUser(int userId)
      {
        return await Client.GetStringAsync(
          $"https://api.vk.com/method/users.get?user_ids={userId}&fields=nickname,first_name,last_name,domain,photo_id&name_case=nom&access_token={_accessToken}&v=5.74");
      }
    public async Task<string> GetUsers(IEnumerable<int> userIds)
    {
      var ids = string.Join(',', userIds);
        return await Client.GetStringAsync(
          $"https://api.vk.com/method/users.get?user_ids={ids}&fields=nickname,first_name,last_name,domain,photo_id&name_case=nom&access_token={_accessToken}&v=5.74");
      }
  }
}
