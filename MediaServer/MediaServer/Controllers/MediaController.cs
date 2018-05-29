using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using MediaServer.Infrastructure;
using MediaServer.Infrastructure.Services;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MediaServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("images")]
  public class MediaController : Controller
  {
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
      _mediaService = mediaService;
    }
    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult("Media Server");
    }

    [HttpPost("isUrlValid")]
    public IActionResult IsUrlValid([FromBody] UrlViewModel url)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      return new OkResponseResult(new {IsValid = _mediaService.IsUrlValid(url.Url)});
    }

    [HttpPost("standard/avatar")]
    public IActionResult GetStandardAvatar([FromBody]AvatarSizeType size)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.GetStandadAvatarUrl(size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("standard/background")]
    public IActionResult GetStandardBackground()
    {
      var url = _mediaService.GetStandadQuestionBackgroundUrl();
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }


    [HttpPost("upload/avatar/link")]
    public IActionResult UploadUserAvatarViaLink([FromBody] UploadAvatarViaLinkViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.UploadAvatar(avatar.Url, avatar.Size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url});
    }

    [HttpPost("upload/background/link")]
    public IActionResult UploadQuestionBackgroundViaLink([FromBody]UploadQuestionBackgroundViaLinkViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.UploadBackground(background.Url);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("upload/avatar/file")]
    public async Task<IActionResult> UploadUserAvatar(UploadUserAvatarViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = await _mediaService.UploadAvatar(avatar.File, avatar.Size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("upload/background/file")]
    public async Task<IActionResult> UploadQuestionBackground(UploadQuestionBackgroundViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = await _mediaService.UploadBackground(background.File);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }
  }
}