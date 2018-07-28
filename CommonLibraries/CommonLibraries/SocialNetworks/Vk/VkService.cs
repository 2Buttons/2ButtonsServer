using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.SocialNetworks.Vk
{
  public class VkService : IVkService
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly ILogger<VkService> _logger;

    private readonly VkAuthSettings _vkAuthSettings;
    private readonly VkAuthSettingsTest _vkAuthSettingsTest;

    public VkService(IOptions<VkAuthSettings> vkAuthSettingsAccessor,
      IOptions<VkAuthSettingsTest> vkAuthSettingsAccessorTest, ILogger<VkService> logger)
    {
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      _vkAuthSettingsTest = vkAuthSettingsAccessorTest.Value;
      _logger = logger;
    }

    public async Task<NormalizedSocialUserData> GetUserInfoAsync(string code, bool isTest = false)
    {
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserInfoAsync)}.Start");
      var accessToken = await GetAccessTokenAsync(code, isTest);
      var result = await GetUserVkInfoAsync(accessToken.ExternalId, accessToken.ExternalEmail, accessToken.AccessToken,
        accessToken.ExpiresIn);
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserInfoAsync)}.End");
      return result;
    }

    public async Task<NormalizedSocialUserData> GetUserInfoAsync(long externalUserId, string email,
      string externalToken, long expiresIn)
    {
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserInfoAsync)}.Start");
      var result = await GetUserVkInfoAsync(externalUserId, email, externalToken, expiresIn);
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserInfoAsync)}.End");
      return result;
    }

    public async Task<VkAppAccessToken> GetAccessTokenAsync(string vkCode, bool isTest = false)
    {
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetAccessTokenAsync)}.Start");
      var vkId = isTest ? _vkAuthSettingsTest.AppId : _vkAuthSettings.AppId;
      var vkAppSecret = isTest ? _vkAuthSettingsTest.AppSecret : _vkAuthSettings.AppSecret;
      var vkRedirectUri = isTest ? _vkAuthSettingsTest.RedirectUri : _vkAuthSettings.RedirectUri;

      var appAccessTokenResponse =
        await Client.GetStringAsync(
          $"https://oauth.vk.com/access_token?client_id={vkId}&client_secret={vkAppSecret}&redirect_uri={vkRedirectUri}&code={vkCode}");
      _logger.LogInformation($"{nameof(GetAccessTokenAsync)} AccessToken: {appAccessTokenResponse}");
      var result = JsonConvert.DeserializeObject<VkAppAccessToken>(appAccessTokenResponse);

      if (!result.Error.IsNullOrEmpty()) throw new Exception(result.Error + " " + result.ErrorDescription);

      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetAccessTokenAsync)}.End");
      return result;
    }

    public async Task<List<VkFriendData>> GetUserFriendsAsync(long vkId)
    {
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserFriendsAsync)}.Start");
      var vkFriendsDataResponse =
        await Client.GetStringAsync(
          $"https://api.vk.com/method/friends.get?user_id={vkId}&lang=0&count={5000}&fields=photo_100&name_case=nom&access_token={_vkAuthSettings.AppAccess}&v=5.80");
      _logger.LogInformation(
        $"{nameof(VkService)}.{nameof(GetUserFriendsAsync)} vkFriendsDataResponse = {vkFriendsDataResponse}");
      var result = JsonConvert.DeserializeObject<VkFriendsDataResponse>(vkFriendsDataResponse).Response.Items.ToList();
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserFriendsAsync)}.End");
      return result;
    }

    private async Task<NormalizedSocialUserData> GetUserVkInfoAsync(long externalUserId, string email,
      string externalToken, long expiresIn)
    {
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserVkInfoAsync)}.Start");
      var userInfoResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/users.get?user_ids={externalUserId}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_max_orig&access_token={externalToken}&v=5.80");
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserInfoAsync)}. UserInfoResponse = {userInfoResponse}");
      var userInfo = JsonConvert.DeserializeObject<VkUserDataResponse>(userInfoResponse).Response.FirstOrDefault();

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
        OriginalPhotoUrl = userInfo.OriginalPhoto
      };
      _logger.LogInformation($"{nameof(VkService)}.{nameof(GetUserVkInfoAsync)}.End");
      return result;
    }
  }
}