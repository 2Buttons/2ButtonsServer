using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CommonLibraries.SocialNetworks.Vk
{
  public class VkService 
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly VkAuthSettings _vkAuthSettings;

    public VkService()
    {
     // _vkAuthSettings = vkAuthSettings;
    }
  

    //public async Task<UserDataFromVk> GetUserInfoAsync(int externalUserId,  string externalToken)
    //{
    //  var userInfoResponse = await Client.GetStringAsync(
    //    $"https://api.vk.com/method/users.get?user_ids={externalUserId}&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
    //  var userInfo = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.FirstOrDefault();

    //  var result = new UserDataFromVk
    //  {
    //    ExternalId = userInfo.UserId,
    //    Login = userInfo.FirstName + " " + userInfo.LastName,
    //    BirthDate = userInfo.Birthday,
    //    SexType = userInfo.Sex,
    //  //  City = await cityName ?? userInfo.City?.Title,
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
          //  City = await cityName ?? userInfo.City?.Title,
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


      //string data;
      //WebRequest request =
      //  WebRequest.CreateHttp(
      //    $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offset*1000}&fields=first_name,last_name,sex,bdate,photo_100,photo_max_orig&access_token={externalToken}&v=5.74");
      //request.Headers.Add("Cookie", $"remixlang={0}");
      //var response = await request.GetResponseAsync();
      //using (var stream = response.GetResponseStream())
      //{
      //  using (var reader = new StreamReader(stream))
      //  {
      //    data = reader.ReadToEnd();
      //  }
      //}
      //response.Close();
      //return data;


      //var items = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.Items;

      //var result = new List<UserDataFromVk>();

      //foreach (var userInfo in items)
      //{
      //  var user = new UserDataFromVk
      //  {
      //    ExternalId = userInfo.UserId,
      //    Login = userInfo.FirstName + " " + userInfo.LastName,
      //    BirthDate = userInfo.Birthday,
      //    SexType = userInfo.Sex,
      //    //  City = await cityName ?? userInfo.City?.Title,
      //    SmallPhotoUrl = userInfo.SmallPhoto,
      //    LargePhotoUrl = userInfo.LargePhoto
      //  };

      //  result.Add(user);
      //}


      //return result;
    }



    //private async Task<string> GetCityNameByIdFromVk(int cityId, LanguageType language)
    //{
    //  string data;
    //  WebRequest request =
    //    WebRequest.CreateHttp(
    //      $"https://api.vk.com/method/database.getCitiesById?city_ids={cityId}&access_token={_vkAuthSettings.AppAccess}&v=5.74");
    //  request.Headers.Add("Cookie", $"remixlang={language}");
    //  var response = await request.GetResponseAsync();
    //  using (var stream = response.GetResponseStream())
    //  {
    //    using (var reader = new StreamReader(stream))
    //    {
    //      data = reader.ReadToEnd();
    //    }
    //  }
    //  response.Close();
    //  return JsonConvert.DeserializeObject<VkCityResponse>(data).Response.FirstOrDefault().Title;
    //}
  }
}