using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using MediaDataLayer;
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

    [HttpPost("uploadUserAvatarViaLink")]
    public async Task<IActionResult> UploadUserAvatarViaLink([FromBody] UploadAvatarViaLinkViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = await _mediaService.UploadAvatar(avatar.UserId, avatar.Url, avatar.Size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url});
    }

    [HttpPost("uploadQuestionBackgroundViaLink")]
    public async Task<IActionResult> UploadQuestionBackgroundViaLink([FromBody]UploadQuestionBackgroundViaLinkViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = await _mediaService.UploadBackground(background.QuestionId, background.Url);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("uploadUserAvatar")]
    public async Task<IActionResult> UploadUserAvatar(UploadUserAvatarViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = await _mediaService.UploadAvatar(avatar.UserId, avatar.File, avatar.Size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("uploadQuestionBackground")]
    public async Task<IActionResult> UploadQuestionBackground(UploadQuestionBackgroundViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = await _mediaService.UploadBackground(background.QuestionId, background.File);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }
  }
}