using CommonLibraries;
using Newtonsoft.Json;

namespace AccountServer.Models.Vk
{
  internal class VkUserDataResponse
  {
    [JsonProperty("response")]
    public VkUserData[] Response { get; set; }
  }

  internal class UsersGetResponse
  {
    [JsonProperty("response")]
    public VkUserData[] Response { get; set; }
  }

  internal class VkCityResponse
  {
    [JsonProperty("response")]
    public VkCity[] Response { get; set; }
  }

  internal class VkFriendIdsResponse
  {
    [JsonProperty("response")]
    public VkFriendItems<int> Response { get; set; }
  }

  internal class VkFriendsDataResponse
  {
    [JsonProperty("response")]
    public VkFriendItems<VkFriendData> Response { get; set; }
  }

  internal class VkFriendItems<T>
  {
    [JsonProperty("count")]
    public int Count { get; set; }
    [JsonProperty("items")]
    public T[] Items { get; set; }
  }

  internal class VkFriendData
  {
    [JsonProperty("id")]
    public int UserId { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("photo_100")]
    public string SmallPhoto { get; set; }
  }

  internal class VkUserData
  {
    private SexType _vkSexType;

    [JsonProperty("id")]
    public int UserId { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("sex")]
    public SexType Sex
    {
      get

      {
        switch (_vkSexType)
        {
          case SexType.Man:
            return SexType.Woman;
          case SexType.Woman:
            return SexType.Man;
          case SexType.Both:
          default:
            return SexType.Both;
        }
      }
      set => _vkSexType = value;
    }

    [JsonProperty("bdate")]
    public string Birthday { get; set; }

    [JsonProperty("city")]
    public VkCity City { get; set; }

    [JsonProperty("photo_100")]
    public string SmallPhoto { get; set; }

    [JsonProperty("photo_max_orig")]
    public string FullPhoto { get; set; }
  }

  internal class VkCity
  {
    [JsonProperty("id")]
    public int CityId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
  }

  internal class VkPictureData
  {
    public VkPicture Data { get; set; }
  }

  internal class VkPicture
  {
    public int Height { get; set; }
    public int Width { get; set; }

    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }

    public string Url { get; set; }
  }

  internal class VkUserAccessTokenData
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

  internal class VkUserAccessTokenValidation
  {
    public VkUserAccessTokenData Data { get; set; }
  }

  internal class VkAppAccessToken
  {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; }
  }
}