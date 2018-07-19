using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CommonLibraries;
using MediaServer.Infrastructure.Services.Configuration;
using MediaServer.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using Size = SixLabors.Primitives.Size;

namespace MediaServer.Infrastructure.Services
{
  public class MediaService
  {
    private readonly FileService _fileManager;
    private readonly FolderConfiguration _folderConfiguration;

    public MediaService(FileService fileManager, FolderConfiguration folderConfiguration)
    {
      _fileManager = fileManager;
      _folderConfiguration = folderConfiguration;
    }

    public bool IsUrlValid(string url)
    {
      return _fileManager.IsUrlValid(url);
    }

    //public List<(AvatarSizeType, string)> GetStandardAvatarUrls()
    //{
    //  var paths = new List<(AvatarSizeType, string)>
    //  {
    //    (AvatarSizeType.Original, _fileManager.ChangePcPathToWeb(_folderConfiguration.Avatars.Standards.Originals
    //      .GetFullHashPath())),
    //    (AvatarSizeType.SmallAvatar, _fileManager.ChangePcPathToWeb(_folderConfiguration.Avatars.Standards.Smalls
    //      .GetFullHashPath())),
    //    (AvatarSizeType.LargeAvatar, _fileManager.ChangePcPathToWeb(_folderConfiguration.Avatars.Standards.Larges
    //      .GetFullHashPath()))
    //  };

    //  return paths;
    //}

    public List<string> GetStandardBackgroundUrls(BackgroundSizeType size)
    {
      var relativeFolder = "";

      switch (size)
      {
        case BackgroundSizeType.Original:
          relativeFolder = _folderConfiguration.Backgrounds.Standards.Originals.GetFullHashPath();
          break;
        case BackgroundSizeType.Mobile:
          relativeFolder = _folderConfiguration.Backgrounds.Standards.Mobiles.GetFullHashPath();
          break;
      }

      var pcPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), relativeFolder);

      return Directory.GetFiles(pcPath)
        .Select(x => _fileManager.ChangePcPathToWeb(Path.Combine(relativeFolder, Path.GetFileName(x)))).ToList();
    }

    //public List<(BackgroundSizeType, string)> GetStandadBackgroundsUrl()
    //{
    //  var paths = new List<(BackgroundSizeType, string)>
    //  {
    //    (BackgroundSizeType.Original, _fileManager.ChangePcPathToWeb(_folderConfiguration.Backgrounds.Standards
    //      .Originals.GetFullHashPath())),
    //    (BackgroundSizeType.Mobile, _fileManager.ChangePcPathToWeb(_folderConfiguration.Backgrounds.Standards.Mobiles
    //      .GetFullHashPath()))
    //  };

    //  return paths;
    //}

    public List<string> GetStandardAvatarUrls(AvatarSizeType size)
    {
      var relativeFolder = "";

      switch (size)
      {
        case AvatarSizeType.Original:
          relativeFolder = _folderConfiguration.Avatars.Standards.Originals.GetFullHashPath();
          break;
        case AvatarSizeType.SmallAvatar:
          relativeFolder = _folderConfiguration.Avatars.Standards.Smalls.GetFullHashPath();
          break;
        case AvatarSizeType.LargeAvatar:
          relativeFolder = _folderConfiguration.Avatars.Standards.Larges.GetFullHashPath();
          break;
      }

      var pcPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), relativeFolder);

      return Directory.GetFiles(pcPath)
        .Select(x => _fileManager.ChangePcPathToWeb(Path.Combine(relativeFolder, Path.GetFileName(x)))).ToList();
    }

    public List<SizedUrl<AvatarSizeType>> UploadAvatar(string url, AvatarType avatarType)
    {
      return UploadAvatar(url, avatarType, x => new WebClient().DownloadFile(new Uri(url), x));
    }

    public List<SizedUrl<AvatarSizeType>> UploadAvatar(IFormFile file, AvatarType avatarType)
    {
      return UploadAvatar(file.FileName, avatarType, x =>
      {
        using (var fileStream = new FileStream(x, FileMode.Create))
        {
          file.CopyTo(fileStream);
        }
      });
    }

    private List<SizedUrl<AvatarSizeType>> UploadAvatar(string fileName, AvatarType avatarType,
      Action<string> saveMethod)
    {
      var imageName = _fileManager.CreateUniqueName(fileName);
      var originalImagePath = CreateRelativeAvatarPath(imageName, avatarType, AvatarSizeType.Original);
      var originalFilePath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), originalImagePath);

      saveMethod.Invoke(originalFilePath);

      var smallImagePath = CreateRelativeAvatarPath(imageName, avatarType ,AvatarSizeType.SmallAvatar);
      var largemagePath = CreateRelativeAvatarPath(imageName, avatarType, AvatarSizeType.LargeAvatar);

      ResizeImage(originalFilePath, Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), smallImagePath), 100, 100);
      ResizeImage(originalFilePath, Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), largemagePath), 600, 600);

      var result = new List<SizedUrl<AvatarSizeType>>
      {
        new SizedUrl<AvatarSizeType>
        {
          Size = AvatarSizeType.Original,
          Url = _fileManager.ChangePcPathToWeb(originalImagePath)
        },
        new SizedUrl<AvatarSizeType>
        {
          Size = AvatarSizeType.SmallAvatar,
          Url = _fileManager.ChangePcPathToWeb(smallImagePath)
        },
        new SizedUrl<AvatarSizeType>
        {
          Size = AvatarSizeType.LargeAvatar,
          Url = _fileManager.ChangePcPathToWeb(largemagePath)
        }
      };

      return result;
    }

    public List<SizedUrl<BackgroundSizeType>> UploadBackground(string url, BackgroundType backgroundType)
    {
      return UploadBackground(url, backgroundType, x => new WebClient().DownloadFile(new Uri(url), x));
    }

    public List<SizedUrl<BackgroundSizeType>> UploadBackground(IFormFile file, BackgroundType backgroundType)
    {
      return UploadBackground(file.FileName, backgroundType,  x =>
      {
        using (var fileStream = new FileStream(x, FileMode.Create))
        {
          file.CopyTo(fileStream);
        }
      });
    }

    private List<SizedUrl<BackgroundSizeType>> UploadBackground(string fileName, BackgroundType backgroundType,
      Action<string> saveMethod)
    {
      var imageName = _fileManager.CreateUniqueName(fileName);
      var originalImagePath = CreateRelativeBackgroundPath(imageName, backgroundType, BackgroundSizeType.Original);
      var originalFilePath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), originalImagePath);

      saveMethod.Invoke(originalFilePath);

      var mobileImagePath = CreateRelativeBackgroundPath(imageName, backgroundType, BackgroundSizeType.Mobile);

      ResizeImage(originalFilePath, Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), mobileImagePath), 200, 512);

      var result = new List<SizedUrl<BackgroundSizeType>>
      {
        new SizedUrl<BackgroundSizeType>
        {
          Size = BackgroundSizeType.Original,
          Url = _fileManager.ChangePcPathToWeb(originalImagePath)
        },
        new SizedUrl<BackgroundSizeType>
        {
          Size = BackgroundSizeType.Mobile,
          Url = _fileManager.ChangePcPathToWeb(mobileImagePath)
        }
      };

      return result;
    }

    public string CreateRelativeAvatarPath(string imageName, AvatarType avatarType, AvatarSizeType size)
    {
      AvatarSizeFolders avatarTypeFolder = null;

      switch (avatarType)
      {
        case AvatarType.Standard:
          avatarTypeFolder = _folderConfiguration.Avatars.Standards;
          break;
        case AvatarType.Custom:
        default:
          avatarTypeFolder = _folderConfiguration.Avatars.Customs;
          break;
      }

      var name = Path.GetFileNameWithoutExtension(imageName);
      var ext = Path.GetExtension(imageName);
      var relativeFolder = "";
      switch (size)
      {
        case AvatarSizeType.Original:
          relativeFolder = avatarTypeFolder.Originals.GetFullHashPath();
          break;
        case AvatarSizeType.SmallAvatar:
          relativeFolder = avatarTypeFolder.Smalls.GetFullHashPath();
          break;
        case AvatarSizeType.LargeAvatar:
          relativeFolder = avatarTypeFolder.Larges.GetFullHashPath();
          break;
      }

      return Path.Combine(relativeFolder, name + $"_{size.ToString().ToLower()}" + ext);
    }

    public string CreateRelativeBackgroundPath(string imageName, BackgroundType backgroundType, BackgroundSizeType size)
    {
      BackgroundSizeFolders backgroundSizeFolders;

      switch (backgroundType)
      {
        case BackgroundType.Standard:
          backgroundSizeFolders = _folderConfiguration.Backgrounds.Standards;
          break;
        case BackgroundType.Custom:
        default:
          backgroundSizeFolders = _folderConfiguration.Backgrounds.Customs;
          break;
      }

      var name = Path.GetFileNameWithoutExtension(imageName);
      var ext = Path.GetExtension(imageName);
      var relativeFolder = "";
      switch (size)
      {
        case BackgroundSizeType.Original:
          relativeFolder = backgroundSizeFolders.Originals.GetFullHashPath();
          break;
        case BackgroundSizeType.Mobile:
          relativeFolder = backgroundSizeFolders.Mobiles.GetFullHashPath();
          break;
      }

      return Path.Combine(relativeFolder, name + $"_{size.ToString().ToLower()}" + ext);
    }

    public bool IsAlreadyDownloadedUrl(string url)
    {
      var uri = new Uri(url);
      return uri.Host.Contains("media.2buttons.ru") && IsUrlValid(uri.AbsolutePath);
    }

    private string ChangeStandardPathToWebPath(string input)
    {
      var indexStandard = input.IndexOf("standards", StringComparison.OrdinalIgnoreCase) - 1;
      var subStr = input.Substring(indexStandard);
      var pathParts = subStr.Split('/', '\\');
      return string.Join('/', pathParts);
    }

    public void ResizeImage(string originalPath, string rezisePath, int height, int width)
    {
      using (var image = Image.Load(originalPath))
      {
        image.Mutate(x => x.Resize(new ResizeOptions
        {
          Mode = ResizeMode.Max,
          Size = new Size {Height = height, Width = width}
        }));

        //var newPath = Path.Combine(Path.GetDirectoryName(originalPath),
        //  Path.GetFileNameWithoutExtension(originalPath) + $"_height={height}&width={width}" +
        //  Path.GetExtension(originalPath));
        image.Save(rezisePath); // Automatic encoder selected based on extension.
      }
    }
  }
}