using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.SocialNetworks.Facebook
{
  public class FbService : IFbService
  {
    private static readonly HttpClient Client = new HttpClient();
    private readonly FacebookAuthSettings _fbAuthSettings;

    public FbService(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
    }

    public async Task<NormalizedSocialUserData> GetUserInfoAsync(string code)
    {
      var accessToken = await GetAccessTokenAsync(code);
      return await GetUserFbInfoAsync(accessToken.AccessToken, accessToken.ExpiresIn);
    }

    public async Task<FacebookAppAccessToken> GetAccessTokenAsync(string fbCode)
    {
      var appAccessTokenResponse =
        await Client.GetStringAsync(
          $"https://graph.facebook.com/v3.0/oauth/access_token?client_id={_fbAuthSettings.AppId}&redirect_uri={_fbAuthSettings.RedirectUri}&client_secret={_fbAuthSettings.AppSecret}&code={fbCode}");
      return JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
    }

    private async Task<NormalizedSocialUserData> GetUserFbInfoAsync(string externalToken, int expiresIn)
    {
      var userInfoResponse = await Client.GetStringAsync(
        $"https://graph.facebook.com/v3.0/me?fields=id,email,first_name,last_name,name,gender,locale,hometown,birthday,picture&access_token={externalToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserResponse>(userInfoResponse);

      //var smallUrlResponse =
      //  await Client.GetStringAsync($"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=normal");
      //var smallUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(smallUrlResponse).Response.Url;

      //var largeUrlResponse =
      //  await Client.GetStringAsync($"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=large");
      //var largeUrl = JsonConvert.DeserializeObject<FacebookPictureResponse>(largeUrlResponse).Response.Url;

      var result = new NormalizedSocialUserData
      {
        ExternalId = userInfo.ExternalId,
        ExternalEmail = userInfo.Email,
        ExternalToken = externalToken,
        ExpiresIn = expiresIn,
        Login = userInfo.FirstName + " " + userInfo.LastName,
        BirthDate = userInfo.Birthday,
        SexType = userInfo.SexType,
        City = userInfo.City?.Title,
        //SmallPhotoUrl = $"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=normal",
        OriginalPhotoUrl = $"https://graph.facebook.com/v3.0/{userInfo.ExternalId}/picture?type=large"
      };
      return result;
    }
  }
}