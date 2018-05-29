using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaServer.Infrastructure.Services
{
  public class MediaService : IMediaService
  {
    private readonly IFileService _fileManager;

    public MediaService(IFileService fileManager)
    {
      _fileManager = fileManager;
    }

    public bool IsUrlValid(string url)
    {
      return _fileManager.IsUrlValid(url);
    }

    public string GetStandadAvatarUrl(AvatarSizeType sizeType)
    {
      var size = sizeType.ToString().Contains("S") ? "small" : "large";
      return $"/standards/{size}_avatar.jpg";
    }

    public string GetStandadQuestionBackgroundUrl()
    {
      return "/standards/question_background.jpg";
    }

    public string UploadAvatar(string url, AvatarSizeType size)
    {
      var imageType = size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(url);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      new WebClient().DownloadFile(new Uri(url), filePath);
      return _fileManager.GetWebPath(imageType, uniqueName);
    }

    public async Task<string> UploadAvatar(IFormFile file, AvatarSizeType size)
    {
      var imageType = size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(file.FileName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(fileStream);
      }
      return _fileManager.GetWebPath(imageType, uniqueName);
    }

    public string UploadBackground(string url)
    {
      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(url);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      new WebClient().DownloadFile(new Uri(url), filePath);
      return _fileManager.GetWebPath(imageType, uniqueName);

    }

    public async Task<string> UploadBackground(IFormFile file)
    {
      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(file.FileName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(fileStream);
      }
      return _fileManager.GetWebPath(imageType, uniqueName);
    }
  }
}