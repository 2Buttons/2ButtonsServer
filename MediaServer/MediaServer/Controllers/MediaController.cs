﻿using System;
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
    private MediaService MediaService { get; }
    private MediaConverter MediaConverter { get; }

    public MediaController(MediaService mediaService, MediaConverter mediaConverter)
    {
      MediaService = mediaService;
      MediaConverter = mediaConverter;
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

      return new OkResponseResult(new {IsValid = MediaService.IsUrlValid(url.Url)});
    }

    [HttpPost("standards/avatar")]
    public IActionResult GetStandardAvatars([FromBody] GetStandardAvatarsViewModel size)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var urls = MediaService.GetStandardAvatarUrls(size.AvatarSizeType);
      return !urls.Any()
        ? new ResponseResult((int) HttpStatusCode.NotFound)
        : new OkResponseResult(new {Urls = urls});
    }

    [HttpPost("standards/background")]
    public IActionResult GetStandardBackgrounds([FromBody] GetStandardBackgroundsViewModel size)
    {
      var urls = MediaService.GetStandardBackgroundUrls(size.BackgroundSizeType);
      return !urls.Any() ? new ResponseResult((int) HttpStatusCode.NotFound) : new OkResponseResult(new {Urls = urls});
    }

    [HttpPost("defaults")]
    public IActionResult GetStandardBackgrounds([FromBody] GetDefaultsViewModel size)
    {
      var urls = MediaService.GetStandardDefaultUrls(size.DefaultSizeType, size.Pattern);
      return !urls.Any() ? new ResponseResult((int)HttpStatusCode.NotFound) : new OkResponseResult(new { Urls = urls });
    }

    [HttpPost("upload/avatar/url")]
    public IActionResult UploadUserAvatarViaUrl([FromBody] UploadAvatarViaUrlViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (MediaService.IsAlreadyDownloaded(avatar.Url))
        return new OkResponseResult(new UrlViewModel { Url = new Uri(MediaConverter.ToFullAvatarUrl(avatar.Url, AvatarSizeType.Original)).LocalPath });
      var url = MediaService.UploadAvatar(avatar.Url, avatar.AvatarType).FirstOrDefault(x => x.Size == AvatarSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/background/url")]
    public IActionResult UploadQuestionBackgroundViaUrl([FromBody] UploadBackgroundViaUrlViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (MediaService.IsAlreadyDownloaded(background.Url))
        return new OkResponseResult(new UrlViewModel {Url = new Uri(MediaConverter.ToFullBackgroundurlUrl(background.Url, BackgroundSizeType.Original)).LocalPath});
      

      var url = MediaService.UploadBackground(background.Url, background.BackgroundType)
        .FirstOrDefault(x => x.Size == BackgroundSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/avatar/file")]
    public IActionResult UploadAvatar(UploadAvatarViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = MediaService.UploadAvatar(avatar.File, avatar.AvatarType).FirstOrDefault(x => x.Size == AvatarSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/background/file")]
    public IActionResult UploadBackground(UploadBackgroundViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = MediaService.UploadBackground(background.File, background.BackgroundType)
        .FirstOrDefault(x => x.Size == BackgroundSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int) HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel {Url = url});
    }

    [HttpPost("upload/default/url")]
    public IActionResult UploadDefaultViaUrl([FromBody] UploadDefaultViaUrlViewModel @default)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (MediaService.IsAlreadyDownloaded(@default.Url))
        return new OkResponseResult(new UrlViewModel { Url = new Uri(MediaConverter.ToFullAvatarUrl(@default.Url, AvatarSizeType.Original)).LocalPath });

      var url = MediaService.UploadDefault(@default.Url)
        .FirstOrDefault(x => x.Size == DefaultSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int)HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("upload/default/file")]
    public IActionResult UploadDefault(UploadDefaultViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = MediaService.UploadDefault(avatar.File).FirstOrDefault(x => x.Size == DefaultSizeType.Original)?.Url;
      return url.IsNullOrEmpty()
        ? new ResponseResult((int)HttpStatusCode.NotModified)
        : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("copy/backgrounds")]
    public IActionResult CopyBackgrounds([FromBody] CopyBackgrounds copyBackgrounds)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      MediaService.CopyBackgrounds(copyBackgrounds.SourceFolder, copyBackgrounds.CopyToNewBackgroundType, copyBackgrounds.CopyToBackgroundSizeType);
      return new OkResponseResult();
    }

    [HttpPost("copy/avatars")]
    public IActionResult CopyAvatars([FromBody] CopyAvatars copyAvatars)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      MediaService.CopyAvatars(copyAvatars.SourceFolder, copyAvatars.CopyToNewAvatarType, copyAvatars.CopyToAvatarSizeType);
      return new OkResponseResult();
    }
  }
}