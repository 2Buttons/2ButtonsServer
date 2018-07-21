using System;
using System.Globalization;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks.Vk;
using CommonTypes;
using Newtonsoft.Json;

namespace CommonLibraries.SocialNetworks.Facebook
{
  public class FacebookUserResponse
  {
    [JsonProperty("id")]
    public long ExternalId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    [JsonProperty("first_name")]
    public string FirstName { get; set; }
    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("gender")]
    public string FbSexType { private get; set; }

    public SexType SexType
    {
      get

      {
        switch (FbSexType)
        {
          case "male":
            return SexType.Man;
          case "female":
            return SexType.Woman;
          default:
            return SexType.Both;
        }
      }
    }
    [JsonProperty("birthday")]
    public string FbBirthday { private get; set; }

    public DateTime Birthday => FbBirthday.IsNullOrEmpty() ? DateTime.Now : DateTime.ParseExact(FbBirthday, "MM/dd/yyyy", CultureInfo.InvariantCulture);


    public string Locale { get; set; }
    [JsonProperty("hometown")]
    public FbCity City { get; set; }
  }

  public class FbCity
  {
    [JsonProperty("id")]
    public int CityId { get; set; }

    [JsonProperty("name")]
    public string Title { get; set; }
  }


  public class FacebookPictureResponse
  {
    [JsonProperty("data")]
    public FacebookPicture Response { get; set; }
  }

  public class FacebookPicture
  {
    public int Height { get; set; }
    public int Width { get; set; }
    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }
    public string Url { get; set; }
  }

  public class FacebookUserAccessTokenResponse
  {
    [JsonProperty("app_id")]
    public long AppId { get; set; }
    public string Type { get; set; }
    public string Application { get; set; }
    [JsonProperty("expires_at")]
    public long ExpiresAt { get; set; }
    [JsonProperty("is_valid")]
    public bool IsValid { get; set; }
    [JsonProperty("user_id")]
    public long UserId { get; set; }
  }

  public class FacebookUserAccessTokenValidation
  {
    public FacebookUserAccessTokenResponse Data { get; set; }
  }

  public class FacebookAppAccessToken
  {
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; }
  }

  public class FbFriendIdsResponse
  {
    [JsonProperty("data")]
    public FbFriendItems<int> Response { get; set; }
  }

  //public class FbFriendsDataResponse
  //{
  //  [JsonProperty("response")]
  //  public FbFriendItems<FbFriendData> Response { get; set; }
  //}

  public class FbFriendItems<T>
  {
    //[JsonProperty("count")]
    //public int Count { get; set; }
    [JsonProperty("uid")]
    public T[] Items { get; set; }
  }
}
