using System;
using System.Linq;
using System.Net;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using MediaServer.Infrastructure.Services;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediaServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("media")]
  public class MediaController : Controller
  {
    private readonly MediaService _mediaService;

    public MediaController(MediaService mediaService)
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
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (string.IsNullOrEmpty(url.Url))
      {
        ModelState.AddModelError("URL", "URL does not have any symbols");
        return new BadResponseResult(ModelState);
      }

      return new OkResponseResult(new {IsValid = _mediaService.IsUrlValid(url.Url)});
    }

    [HttpPost("standards/avatar")]
    public IActionResult GetStandardAvatars([FromBody] GetStandardAvatarsViewModel size)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var urls = _mediaService.GetStandardAvatarUrls(size.SizeType);
      return !urls.Any()
        ? new ResponseResult((int) HttpStatusCode.NotFound)
        : new OkResponseResult(new {Urls = urls});
    }

    [HttpPost("standards/background")]
    public IActionResult GetStandardBackgrounds([FromBody] GetStandardBackgroundsViewModel size)
    {
      var urls = _mediaService.GetStandardBackgroundUrls(size.SizeType);
      return !urls.Any() ? new ResponseResult((int) HttpStatusCode.NotFound) : new OkResponseResult(new {Urls = urls});
    }

    [HttpPost("defaults")]
    public IActionResult GetStandardBackgrounds([FromBody] GetDefaultsViewModel size)
    {
      var urls = _mediaService.GetStandardDefaultUrls(size.SizeType, size.Pattern);
      return !urls.Any() ? new ResponseResult((int)HttpStatusCode.NotFound) : new OkResponseResult(new { Urls = urls });
    }

    [HttpPost("upload/avatar/url")]
    public IActionResult UploadUserAvatarViaUrl([FromBody] UploadAvatarViaUrlViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (_mediaService.IsAlreadyDownloaded(avatar.Url))
        return new OkResponseResult(new UrlViewModel { Url = new Uri(MediaConverter.ToFullAvatarUrl(avatar.Url, AvatarSizeType.Original)).LocalPath });
      var url = _mediaService.UploadAvatar(avatar.Url, avatar.AvatarType).FirstOrDefault(x => x.Size == AvatarSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/background/url")]
    public IActionResult UploadQuestionBackgroundViaUrl([FromBody] UploadBackgroundViaUrlViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (_mediaService.IsAlreadyDownloaded(background.Url))
        return new OkResponseResult(new UrlViewModel {Url = new Uri(MediaConverter.ToFullBackgroundurlUrl(background.Url, BackgroundSizeType.Original)).LocalPath});
      

      var url = _mediaService.UploadBackground(background.Url, background.BackgroundType)
        .FirstOrDefault(x => x.Size == BackgroundSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/avatar/file")]
    public IActionResult UploadAvatar(UploadAvatarViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.UploadAvatar(avatar.File, avatar.AvatarType).FirstOrDefault(x => x.Size == AvatarSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/background/file")]
    public IActionResult UploadBackground(UploadBackgroundViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = _mediaService.UploadBackground(background.File, background.BackgroundType)
        .FirstOrDefault(x => x.Size == BackgroundSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/default/url")]
    public IActionResult UploadDefaultViaUrl([FromBody] UploadDefaultViaUrlViewModel @default)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (_mediaService.IsAlreadyDownloaded(@default.Url))
        return new OkResponseResult(new UrlViewModel { Url = new Uri(MediaConverter.ToFullAvatarUrl(@default.Url, AvatarSizeType.Original)).LocalPath });

      var url = _mediaService.UploadDefault(@default.Url)
        .FirstOrDefault(x => x.Size == DefaultSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int)HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("upload/default/file")]
    public IActionResult UploadDefault(UploadDefaultViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.UploadDefault(avatar.File).FirstOrDefault(x => x.Size == DefaultSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int)HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel { Url = url });
    }
  }
}