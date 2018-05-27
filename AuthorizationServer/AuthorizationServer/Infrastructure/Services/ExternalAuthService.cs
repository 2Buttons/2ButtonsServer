using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;
using AuthorizationServer.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Newtonsoft.Json;

namespace AuthorizationServer.Infrastructure.Services
{
  public class ExternalAuthService : IDisposable
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IJwtService _jwtService;
    private readonly IVkService _vkService;
    private readonly IFbService _fbService;

    public ExternalAuthService(IJwtService jwtService, AuthorizationUnitOfWork db, IVkService vkService, IFbService fbService)
    {
      _db = db;
      _jwtService = jwtService;
      _vkService = vkService;
      _fbService = fbService;
    }

    public void Dispose()
    {
      _db.Dispose();
    }

    public void ExternalLogin(string code, SocialType socialType)
    {
      NormalizedSocialData
    }

    private async Task<(string, string)> UploadAvatars(int userId, string smallPhotoUrl, string fullPhotoUrl)
    {
      var jsonSmall = JsonConvert.SerializeObject(new { userId, size = 0, url = smallPhotoUrl });
      var jsonFull = JsonConvert.SerializeObject(new { userId, size = 1, url = fullPhotoUrl });
      var s = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonSmall);
      var f = UploadPhotoViaLink("http://localhost:6250/images/uploadUserAvatarViaLink", jsonFull);

      await Task.WhenAll(f, s);
      return (f.Result, s.Result);
    }

    private static async Task<string> UploadPhotoViaLink(string url, string requestJson)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(requestJson);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
