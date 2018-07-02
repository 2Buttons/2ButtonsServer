using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataGenerator.VkCrawler
{
  public class VkService
  {
    private const string AppId = "6469856";
    private const string AppSecret = "Zcec8RyHjpvaVfvRxLvq";
    private const string AppAccess = "8ca841528ca841528ca84152578ccaf9b288ca88ca84152d645d207a5fae99916d1944a";
    private static readonly HttpClient Client = new HttpClient();

    public async Task<List<VkUserData>> GetUsersFromGroup(int groupId, int count, string externalToken)
    {
      string userInfoResponse;
      var result = new List<VkUserData>();
      if (count < 1000)
      {
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={0}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
        return JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.Items.ToList();
      }
      for (var i = 0; i < count / 1000; i++)
      {
        var offset = i * 1000;
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offset}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
        result.AddRange(JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.Items.ToList());
        Thread.Sleep(4000);
      }

      return result;
    }

    public async Task WriteMemberGroupsToFile(string groupId, int count, string externalToken, string folderPath)
    {
      var path = folderPath + "_" + groupId + ".txt";
      string jsonUsers;
      string userInfoResponse;
      var result = new List<VkUserData>();
      if (count < 1000)
      {
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={0}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
        jsonUsers = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse)
          .Response.Items.ToList());

        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
        {
          sw.WriteLine(jsonUsers);
        }
        return;
      }
      for (var i = 0; i < count / 1000; i++)
      {
        var offset = i * 1000;
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offset}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
        result.AddRange(JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.Items.ToList());
        Thread.Sleep(4000);
      }
      jsonUsers = JsonConvert.SerializeObject(result);
      using (var sw = new StreamWriter(path, false, Encoding.UTF8))
      {
        sw.WriteLine(jsonUsers);
      }
    }

    public async Task<string> GetJsonFromGroup(string groupId, int offset, string externalToken)
    {
      var offSet = offset * 1000;
      return await Client.GetStringAsync(
        $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offSet}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
    }
  }
}