using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class MediaServerHelper
  {

    private const string UploadedAvaterUrl = "http://localhost:15009/media/upload/avatar/link";
    private const string UploadedAvaterFile = "http://localhost:15009/media/upload/avatar/file";
    private const string StandardAvatarUrl = "http://localhost:15009/media/standards/avatar/";

    private const string UploadedBackgroundUrl = "http://localhost:15009/media/upload/background/link";
    private const string UploadedBackgroundFile = "http://localhost:15009/media/upload/background/file";
    private const string StandardBackgroundUrl = "http://localhost:15009/media/standards/background/";

    public static string StandardAvatar(AvatarSizeType sizeType)
    {
      var size = sizeType.ToString().Contains("S") ? "small" : "large";
      return $"/standards/{size}_avatar.jpg";
    }
    public static string StandardBackground() => "/standards/question_background.jpg";

    public static async Task<string> GetStandardAvatarUrl(AvatarSizeType avatarSize)
    {
      try
      {
        var body = JsonConvert.SerializeObject(new { size = (int)avatarSize });
        return (await MediaServerConnection(StandardAvatarUrl, body)).Url;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return string.Empty;
    }

    public static async Task<string> UploadAvatarFile(AvatarSizeType avatarSize, IFormFile file)
    {
      try
      {
        var requestParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("size", ((int)avatarSize).ToString()) };
        return (await MediaServerConnection(UploadedAvaterFile, file, requestParams)).Url;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return string.Empty;
    }

    public static async Task<string> UploadAvatarUrl(AvatarSizeType avatarSize, string imageUrl)
    {
      try
      {
        var body = JsonConvert.SerializeObject(new { size = (int)avatarSize, url = imageUrl });
        return (await MediaServerConnection(UploadedAvaterUrl, body)).Url;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return string.Empty;
    }

    public static async Task<List<string>> GetStandardBackgroundsUrl()
    {
      try
      {
        var body = JsonConvert.SerializeObject(new { });
        return (await MediaServerConnection(StandardBackgroundUrl, body)).Urls;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return new List<string>();
    }

    public static async Task<string> UploadBackgroundUrl(string imageUrl)
    {
      try
      {
        var body = JsonConvert.SerializeObject(new { url = imageUrl });
        return (await MediaServerConnection(UploadedBackgroundUrl, body)).Url;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return string.Empty;
    }

    public static async Task<string> UploadBackgroundFile(IFormFile file)
    {
      try
      {
        return (await MediaServerConnection(UploadedBackgroundFile, file, new List<KeyValuePair<string, string>>())).Url;
      }
      catch (Exception ex)
      {
        //TODO
        // убрать такое!!!! логирование нормально провести
        // ignored
      }
      return string.Empty;
    }

    private static async Task<UrlReponse> MediaServerConnection(string url, string body)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(body);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        var result = reader.ReadToEnd();
        return MediaResponseToUrl(result);
      }
    }

    private static async Task<UrlReponse> MediaServerConnection(string url, IFormFile file, IEnumerable<KeyValuePair<string, string>> requestParams)
    {
      var client = new HttpClient();
      var content = new MultipartFormDataContent
      {
        new FormUrlEncodedContent(requestParams)
      };
      using (var fstream = file.OpenReadStream())
      {
        var streamContent = new StreamContent(fstream);
        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
          Name = "file"
          ,
          FileName = file.FileName
        };
        content.Add(streamContent);

        HttpResponseMessage response = await client.PostAsync(url, content);
        if (response.StatusCode != HttpStatusCode.OK) return null;
        var result = await response.Content.ReadAsStringAsync();
        return MediaResponseToUrl(result);
      }

    }

    private static UrlReponse MediaResponseToUrl(string response)
    {
      return JsonConvert.DeserializeObject<ResponseObject<UrlReponse>>(response).Data;
    }
    private class UrlReponse
    {
      [JsonProperty("url")]
      public string Url { get; set; }

      [JsonProperty("urls")]
      public List<string> Urls { get; set; }
    }
  }
}
