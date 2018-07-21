using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.SocialNetworks.Vk
{
  public class VkService : IVkService
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly VkAuthSettings _vkAuthSettings;
    private readonly VkAuthSettingsTest _vkAuthSettingsTest;

    public VkService(IOptions<VkAuthSettings> vkAuthSettingsAccessor, IOptions<VkAuthSettingsTest> vkAuthSettingsAccessorTest)
    {
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      _vkAuthSettingsTest = vkAuthSettingsAccessorTest.Value;
    }

    public async Task<NormalizedSocialUserData> GetUserInfoAsync(string code, bool isTest = false)
    {
      var accessToken = await GetAccessTokenAsync(code, isTest);
      return await GetUserVkInfoAsync(accessToken.ExternalId, accessToken.ExternalEmail, accessToken.AccessToken, accessToken.ExpiresIn);
    }

    public async Task<NormalizedSocialUserData> GetUserInfoAsync(long externalUserId, string email, string externalToken, long expiresIn)
    {
      return await GetUserVkInfoAsync(externalUserId, email, externalToken, expiresIn);
    }

    public async Task<VkAppAccessToken> GetAccessTokenAsync(string vkCode, bool isTest = false)
    {
      var vkId = isTest ? _vkAuthSettingsTest.AppId : _vkAuthSettings.AppId;
      var vkAppSecret = isTest ? _vkAuthSettingsTest.AppSecret : _vkAuthSettings.AppSecret;
      var vkRedirectUri = isTest ? _vkAuthSettingsTest.RedirectUri : _vkAuthSettings.RedirectUri;
      var appAccessTokenResponse =
        await Client.GetStringAsync(
          $"https://oauth.vk.com/access_token?client_id={vkId}&client_secret={vkAppSecret}&redirect_uri={vkRedirectUri}&code={vkCode}");
      var result = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);
      if (!result.Error.IsNullOrEmpty())
        throw new Exception(result.Error + " " + result.ErrorDescription);
      return result;
    }

    async Task<NormalizedSocialUserData> GetUserVkInfoAsync(long externalUserId, string email, string externalToken, long expiresIn)
    {
      var userInfoResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/users.get?user_ids={externalUserId}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");
      var userInfo = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.FirstOrDefault();

      //var cityName = GetCityNameByIdFromVk(userInfo.City.CityId, LanguageType.Russian);
      //userInfo.City.Title = await cityName ?? userInfo.City.Title;

      var result = new NormalizedSocialUserData
      {
        ExternalId = userInfo.UserId,
        ExternalEmail = email,
        ExternalToken = externalToken,
        ExpiresIn = expiresIn,
        Login = userInfo.FirstName + " " + userInfo.LastName,
        BirthDate = userInfo.Birthday,
        SexType = userInfo.Sex,
        City = userInfo.City?.Title,
        //SmallPhotoUrl = userInfo.SmallPhoto,
        OriginalPhotoUrl = userInfo.OriginalPhoto
      };
      return result;
    }

    public async Task<List<VkFriendData>> GetUserFriendsAsync(long vkId)
    {
      var vkFriendsDataResponse =
        await Client.GetStringAsync(
          $"https://api.vk.com/method/friends.get?user_id={vkId}&lang=0&count={5000}&fields=photo_100&name_case=nom&access_token={_vkAuthSettings.AppAccess}&v=5.80");
      return JsonConvert.DeserializeObject<VkFriendsDataResponse>(vkFriendsDataResponse).Response.Items.ToList();
    }

    //private async Task<string> GetCityNameByIdFromVk(int cityId, LanguageType language)
    //{
    //  string data;
    //  WebRequest request =
    //    WebRequest.CreateHttp(
    //      $"https://api.vk.com/method/database.getCitiesById?city_ids={cityId}&lang=0&access_token={_vkAuthSettings.AppAccess}&v=5.80");
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