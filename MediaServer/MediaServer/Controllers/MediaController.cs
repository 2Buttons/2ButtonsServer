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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace MediaServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("media")]
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

    [HttpPost("standards/avatar")]
    public IActionResult GetStandardAvatar([FromBody]AvatarSizeType size)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = _mediaService.GetStandadAvatarUrl(size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url });
    }

    [HttpPost("standards/background")]
    public IActionResult GetStandardBackgrounds()
    {
      var urls = _mediaService.GetQuestionStandadBackgroundsUrl();
      return urls.Count<1 ? new ResponseResult((int)HttpStatusCode.NotFound) : new OkResponseResult(new { Urls = urls });
    }


    [HttpPost("upload/avatar/link")]
    public IActionResult UploadUserAvatarViaLink([FromBody] UploadAvatarViaLinkViewModel avatar)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (_mediaService.IsAlreadyDownloadedUrl(avatar.Url))
        return new OkResponseResult(new UrlViewModel { Url = avatar.Url });
      var url = _mediaService.UploadAvatar(avatar.Url, avatar.Size);
      return url.IsNullOrEmpty() ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult(new UrlViewModel { Url = url});
    }

    [HttpPost("upload/background/link")]
    public IActionResult UploadQuestionBackgroundViaLink([FromBody]UploadQuestionBackgroundViaLinkViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (_mediaService.IsAlreadyDownloadedUrl(background.Url))
        return new OkResponseResult(new UrlViewModel {Url = background.Url });

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

    public void REsize()
    {
      using (Image<Rgba32> image = Image.Load("foo.jpg")) //open the file and detect the file type and decode it
      {
        // image is now in a file format agnostic structure in memory as a series of Rgba32 pixels
        image.Mutate(ctx => ctx.Resize(image.Width / 2, image.Height / 2)); // resize the image in place and return it for chaining
        image.Save("bar.jpg"); // based on the file extension pick an encoder then encode and write the data to disk
      } // dispose - releasing memory into a memory pool ready for the next image you wish to process
    }
  }
}