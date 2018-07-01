using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataGenerator.VkCrawler
{
  public class VkService 
  {
    private static readonly HttpClient Client = new HttpClient();

    private const string  AppId = "6469856";
    private const string AppSecret = "Zcec8RyHjpvaVfvRxLvq";
    private const string AppAccess = "8ca841528ca841528ca84152578ccaf9b288ca88ca84152d645d207a5fae99916d1944a";

    //public async Task<UserDataFromVk> GetUserInfoAsync(int externalUserId, string externalToken)
    //{
    //  var userInfoResponse = await Client.GetStringAsync(
    //    $"https://api.vk.com/method/users.get?user_ids={externalUserId}&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
    //  var userInfo = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response;

    //  var result = new UserDataFromVk
    //  {
    //    ExternalId = userInfo.UserId,
    //    Login = userInfo.FirstName + " " + userInfo.LastName,
    //    BirthDate = userInfo.Birthday,
    //    SexType = userInfo.Sex,
    //    //  City = await cityName ?? userInfo.City?.Title,
    //    SmallPhotoUrl = userInfo.SmallPhoto,
    //    LargePhotoUrl = userInfo.LargePhoto
    //  };
    //  return result;
    //}

    public async Task<List<UserDataFromVk>> GetUsersFromGroup(int groupId, string externalToken)
    {
      var userInfoResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&fields=first_name,last_name,sex,bdate,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
      var items = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.Items;

      var result = new List<UserDataFromVk>();

      foreach (var userInfo in items)
      {
        var user = new UserDataFromVk
        {
          ExternalId = userInfo.UserId,
          Login = userInfo.FirstName + " " + userInfo.LastName,
          BirthDate = userInfo.Birthday,
          SexType = userInfo.Sex,
          City = userInfo.City?.Title,
          SmallPhotoUrl = userInfo.SmallPhoto,
          LargePhotoUrl = userInfo.LargePhoto
        };

        result.Add(user);
      }

      
      return result;
    }

    public async Task<string> GetJsonFromGroup(string groupId, int offset, string externalToken)
    {
      var off_set = offset * 1000;
      return await Client.GetStringAsync(
        $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={off_set}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");


      
    }
  }
}