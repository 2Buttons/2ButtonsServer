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

    public const string UploadedAvaterUrl = "http://localhost:6250/upload/avatar/link";
    public const string UploadedAvaterFile = "http://localhost:6250/upload/avatar/file";
    public const string StandardAvatarUrl = "http://localhost:6250/standard/avatar/";

    public const string UploadedBackgroundFile = "http://localhost:6250/upload/background/link";
    public const string UploadedBackgroundUrl = "http://localhost:6250/upload/background/file";
    public const string StandardBackgroundUrl = "http://localhost:6250/standard/background/";

    public static async Task<string> GetStandardAvatarUrl(AvatarSizeType avatarSize)
    {
      var body = JsonConvert.SerializeObject(new { size = (int)avatarSize });
      return await MediaServerConnection(StandardAvatarUrl, body);
    }

    public static async Task<string> UploadAvatarFile(AvatarSizeType avatarSize, IFormFile file)
    {
      var requestParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("size", ((int)avatarSize).ToString()) };
      return await MediaServerConnection(UploadedAvaterFile, file, requestParams);
    }

    public static async Task<string> UploadAvatarUrl(AvatarSizeType avatarSize, string url)
    {
      var body = JsonConvert.SerializeObject(new { size = 0, url = url });
      return await MediaServerConnection(UploadedAvaterUrl, body);
    }

    public static async Task<string> GetStandardBackgroundUrl()
    {
      var body = JsonConvert.SerializeObject(new { });
      return await MediaServerConnection(StandardBackgroundUrl, body);
    }

    public static async Task<string> UploadBackgroundUrl(string url)
    {
      var body = JsonConvert.SerializeObject(new { url });
      return await MediaServerConnection(UploadedBackgroundUrl, body);
    }

    public static async Task<string> UploadBackgroundFile(IFormFile file)
    {
      return await MediaServerConnection(UploadedBackgroundFile, file, new List<KeyValuePair<string, string>>());
    }

    private static async Task<string> MediaServerConnection(string url, string body)
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
        return reader.ReadToEnd();
      }
    }

    private static async Task<string> MediaServerConnection(string url, IFormFile file, IEnumerable<KeyValuePair<string, string>> requestParams)
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
        if (response.StatusCode != HttpStatusCode.OK) return string.Empty;
        var data = JsonConvert.DeserializeObject<ResponseObject>(await response.Content.ReadAsStringAsync()).Data;

        return data is UrlReponse reponse ? reponse.Url : string.Empty;
      }

    }
    private class UrlReponse
    {
      public string Url { get; set; }
    }
  }
}
