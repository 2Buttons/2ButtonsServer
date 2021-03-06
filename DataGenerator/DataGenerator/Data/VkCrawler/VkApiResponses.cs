﻿using System;
using CommonLibraries;
using Newtonsoft.Json;

namespace DataGenerator.Data.VkCrawler
{
  public class VkUserDataResponse
  {
    [JsonProperty("response")]
    public VkItemsResponse Response { get; set; }
  }

  public class UsersGetResponse
  {
    [JsonProperty("response")]
    public VkUserData[] Response { get; set; }
  }

  public class VkCityResponse
  {
    [JsonProperty("response")]
    public VkItems<VkCity> Response { get; set; }
  }

  public class VkFriendIdsResponse
  {
    [JsonProperty("response")]
    public VkItems<int> Response { get; set; }
  }

  public class VkFriendsDataResponse
  {
    [JsonProperty("response")]
    public VkItems<VkFriendData> Response { get; set; }
  }

  public class VkItems<T>
  {
    [JsonProperty("count")]
    public int Count { get; set; }
    [JsonProperty("items")]
    public T[] Items { get; set; }
  }

  public class VkFriendData
  {
    [JsonProperty("id")]
    public int ExternalId { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("photo_100")]
    public string SmallPhoto { get; set; }
  }

  public class VkItemsResponse
  {
    [JsonProperty("items")]
    public VkUserData[] Items { get; set; }
  }


  public class VkUserData
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
    public string VkBirthday { private get; set; }

    public DateTime Birthday => ParseDateTime(VkBirthday);

    private DateTime ParseDateTime(string str)
    {
      if (string.IsNullOrEmpty(str)) return DateTime.UtcNow;
      if (DateTime.TryParse(str, out var result)) return result;
      return DateTime.UtcNow;
    }

    [JsonProperty("city")]
    public VkCity City { get; set; }

    [JsonProperty("photo_100")]
    public string SmallPhoto { get; set; }

    [JsonProperty("photo_max_orig")]
    public string LargePhoto { get; set; }
  }

  public class VkCity
  {
    [JsonProperty("id")]
    public int CityId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
  }

  public class VkPictureData
  {
    public VkPicture Data { get; set; }
  }

  public class VkPicture
  {
    public int Height { get; set; }
    public int Width { get; set; }

    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }

    public string Url { get; set; }
  }

  public class VkUserAccessTokenData
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

  public class VkUserAccessTokenValidation
  {
    public VkUserAccessTokenData Data { get; set; }
  }

  public class VkAppAccessToken
  {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("user_id")]
    public int ExternalId { get; set; }

    [JsonProperty("email")]
    public string ExternalEmail { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; }
  }
}