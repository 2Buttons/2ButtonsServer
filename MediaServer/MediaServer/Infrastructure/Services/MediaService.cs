using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MediaDataLayer;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaServer.Infrastructure.Services
{
  public class MediaService : IMediaService
  {
    private readonly IFileService _fileManager;
    private readonly MediaUnitOfWork _mainDb;

    public MediaService(IFileService fileManager, MediaUnitOfWork mainDb1)
    {
      _fileManager = fileManager;
      _mainDb = mainDb1;
    }

    public bool IsUrlValid(string url)
    {
      return _fileManager.IsUrlValid(url);
    }

    public async Task<string> UploadAvatar(int userId, string url, AvatarSizeType size)
    {
      var imageType = size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(url);
      var avatarLink = _fileManager.GetWebPath(imageType, uniqueName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      var isUpdated = false;
      switch (size)
      {
        case AvatarSizeType.UserSmallAvatarPhoto:
          isUpdated = await _mainDb.Accounts.UpdateUserSmallAvatar(userId, avatarLink);
          break;
        case AvatarSizeType.UserFullAvatarPhoto:
          isUpdated = await _mainDb.Accounts.UpdateUserFullAvatar(userId, avatarLink);
          break;
      }

      if (!isUpdated) return string.Empty;

      new WebClient().DownloadFileAsync(new Uri(url), filePath);
      return avatarLink;
    }

    public async Task<string> UploadAvatar(int userId, IFormFile file, AvatarSizeType size)
    {
      var imageType = size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(file.FileName);
      var avatarLink = _fileManager.GetWebPath(imageType, uniqueName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      var isUpdated = false;
      switch (size)
      {
        case AvatarSizeType.UserSmallAvatarPhoto:
          isUpdated = await _mainDb.Accounts.UpdateUserSmallAvatar(userId, avatarLink);
          break;
        case AvatarSizeType.UserFullAvatarPhoto:
          isUpdated = await _mainDb.Accounts.UpdateUserFullAvatar(userId, avatarLink);
          break;
      }

      if (!isUpdated) return string.Empty;

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(fileStream);
      }
      return avatarLink;
    }

    public async Task<string> UploadBackground(int questionId, string url)
    {
      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(url);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      var backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
      if (await _mainDb.Questions.UpdateQuestionBackgroundLink(questionId, backgroundLink))
      {
        new WebClient().DownloadFileAsync(new Uri(url), filePath);
        return backgroundLink;
      }
      return string.Empty;
    }

    public async Task<string> UploadBackground(int questionId, IFormFile file)
    {
      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(file.FileName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      var backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
      if (await _mainDb.Questions.UpdateQuestionBackgroundLink(questionId, backgroundLink))
      {
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(fileStream);
        }
        return backgroundLink;
      }
      return string.Empty;
    }
  }
}