using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibraries.Response;
using CommonTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.ConnectionServices
{
  public class MediaServerConnection
  {
    private readonly ILogger<MediaServerConnection> _logger;
    private readonly string _standardAvatarUrl;
    private readonly string _standardBackgroundUrl;
    private readonly string _uploadedAvaterFile;

    private readonly string _uploadedAvaterUrl;
    private readonly string _uploadedBackgroundFile;

    private readonly string _uploadedBackgroundUrl;

    public MediaServerConnection(IOptions<ServersSettings> mediaOptions, ILogger<MediaServerConnection> logger)
    {
      _logger = logger;

      _uploadedAvaterUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/avatar/link";
      _uploadedAvaterFile = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/avatar/file";
      _standardAvatarUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/standards/avatar/";

      _uploadedBackgroundUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/background/link";
      _uploadedBackgroundFile = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/background/file";
      _standardBackgroundUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/standards/background/";

      _uploadedBackgroundUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/default/link";
      _uploadedBackgroundFile = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/upload/default/file";
      _standardBackgroundUrl = $"http://localhost:{mediaOptions.Value["Media"].Port}/media/default";
    }



    public async Task<List<string>> GetStandardAvatarUrls(AvatarSizeType avatarSize)
    {
      try
      {
        _logger.LogInformation($"{nameof(GetStandardAvatarUrls)}.Start");
        var body = JsonConvert.SerializeObject(new {size = (int) avatarSize});
        var result = (await MediaServerConnect(_standardAvatarUrl, body)).Urls;
        _logger.LogInformation($"{nameof(GetStandardAvatarUrls)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(GetStandardAvatarUrls)}");
      }
      return new List<string>();
    }

    public async Task<List<string>> GetStandardBackgroundsUrl(BackgroundSizeType backgroundSize)
    {
      try
      {
        _logger.LogInformation($"{nameof(GetStandardBackgroundsUrl)}.Start");
        var body = JsonConvert.SerializeObject(new { size = (int)backgroundSize } );
        var result = (await MediaServerConnect(_standardBackgroundUrl, body)).Urls;
        _logger.LogInformation($"{nameof(GetStandardBackgroundsUrl)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(GetStandardBackgroundsUrl)}");
      }
      return new List<string>();
    }

    public async Task<List<string>> GetStandardDefaultsUrl(DefaultSizeType defaultSize, string pattern)
    {
      try
      {
        _logger.LogInformation($"{nameof(GetStandardBackgroundsUrl)}.Start");
        var body = JsonConvert.SerializeObject(new { size = (int)defaultSize, pattern = pattern });
        var result = (await MediaServerConnect(_standardBackgroundUrl, body)).Urls;
        _logger.LogInformation($"{nameof(GetStandardBackgroundsUrl)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(GetStandardBackgroundsUrl)}");
      }
      return new List<string>();
    }


    public async Task<string> UploadAvatarFile(AvatarType  avatarType, IFormFile file)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadAvatarFile)}.Start");
        var requestParams =
          new List<KeyValuePair<string, string>>
          {
            new KeyValuePair<string, string>("avatarType", ((int) avatarType).ToString())
          };
        var result = (await MediaServerConnect(_uploadedAvaterFile, file, requestParams)).Url;
        _logger.LogInformation($"{nameof(UploadAvatarFile)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadAvatarFile)}");
      }
      return string.Empty;
    }

    public async Task<string> UploadAvatarUrl(AvatarType avatarType, string imageUrl)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadAvatarUrl)}.Start");
        var body = JsonConvert.SerializeObject(new { avatarType = (int)avatarType, url = imageUrl});
        var result = (await MediaServerConnect(_uploadedAvaterUrl, body)).Url;
        _logger.LogInformation($"{nameof(UploadAvatarUrl)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadAvatarUrl)}");
      }
      return string.Empty;
    }

   
    public async Task<string> UploadBackgroundUrl(BackgroundType backgroundType, string imageUrl)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadBackgroundUrl)}.Start");
        var body = JsonConvert.SerializeObject(new { backgroundType = (int)backgroundType, url = imageUrl});
        var result = (await MediaServerConnect(_uploadedBackgroundUrl, body)).Url;
        _logger.LogInformation($"{nameof(UploadBackgroundUrl)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadBackgroundUrl)}");
      }
      return string.Empty;
    }

    public async Task<string> UploadBackgroundFile(BackgroundType backgroundType, IFormFile file)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadBackgroundFile)}.Start");
        var requestParams =
          new List<KeyValuePair<string, string>>
          {
            new KeyValuePair<string, string>("backgroundType", ((int) backgroundType).ToString())
          };
        var result = (await MediaServerConnect(_uploadedBackgroundFile, file, requestParams))
          .Url;
        _logger.LogInformation($"{nameof(UploadBackgroundFile)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadBackgroundFile)}");
      }
      return string.Empty;
    }


    public async Task<string> UploadDefaultUrl(string imageUrl)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadBackgroundUrl)}.Start");
        var body = JsonConvert.SerializeObject(new {  url = imageUrl });
        var result = (await MediaServerConnect(_uploadedBackgroundUrl, body)).Url;
        _logger.LogInformation($"{nameof(UploadBackgroundUrl)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadBackgroundUrl)}");
      }
      return string.Empty;
    }

    public async Task<string> UploadDefaultFile(IFormFile file)
    {
      try
      {
        _logger.LogInformation($"{nameof(UploadBackgroundFile)}.Start");
        var result = (await MediaServerConnect(_uploadedBackgroundFile, file, new List<KeyValuePair<string, string>>()))
          .Url;
        _logger.LogInformation($"{nameof(UploadBackgroundFile)}.Start");
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(UploadBackgroundFile)}");
      }
      return string.Empty;
    }

    private async Task<UrlReponse> MediaServerConnect(string url, string body)
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

    private static async Task<UrlReponse> MediaServerConnect(string url, IFormFile file,
      IEnumerable<KeyValuePair<string, string>> requestParams)
    {
      var client = new HttpClient();
      var content = new MultipartFormDataContent {new FormUrlEncodedContent(requestParams)};
      using (var fstream = file.OpenReadStream())
      {
        var streamContent = new StreamContent(fstream);
        streamContent.Headers.ContentDisposition =
          new ContentDispositionHeaderValue("form-data") {Name = "file", FileName = file.FileName};
        content.Add(streamContent);

        var response = await client.PostAsync(url, content);
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