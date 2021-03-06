﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataGenerator.Data.VkCrawler
{
  public class VkService
  {
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
      var path = folderPath + "VkUsers_" + groupId + ".txt";
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

    public async Task WriteMemberGroupsStringToFile(string groupId, int count, string externalToken, string folderPath)
    {
      var path = folderPath + "VkUsers_" + groupId + ".txt";
      string userInfoResponse;

      if (count < 1000)
      {
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={0}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");

        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
        {
          sw.WriteLine(userInfoResponse);
        }
        return;
      }
      for (var i = 0; i < count / 1000; i++)
      {
        path = folderPath + "VkUsers_" + groupId + $"_{i}_" + ".txt";
        var offset = i * 1000;
        userInfoResponse = await Client.GetStringAsync(
          $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offset}&lang=0&sort=id_desc&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
        {
          sw.WriteLine(userInfoResponse);
        }
        Thread.Sleep(4000);
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