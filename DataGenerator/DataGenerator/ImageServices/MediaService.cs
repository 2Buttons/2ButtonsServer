using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders.Configurations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace DataGenerator.Services
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

    public List<string> GetStandardAvatarUrls(AvatarSizeType size)
    {
      var relativeFolder = "";

      switch (size)
      {
        case AvatarSizeType.Original:
          relativeFolder = _folderConfiguration.Avatars.Standards.Originals.GetFullHashPath();
          break;
        case AvatarSizeType.Small:
          relativeFolder = _folderConfiguration.Avatars.Standards.Smalls.GetFullHashPath();
          break;
        case AvatarSizeType.Large:
          relativeFolder = _folderConfiguration.Avatars.Standards.Larges.GetFullHashPath();
          break;
      }

      var pcPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), relativeFolder);

      return Directory.GetFiles(pcPath)
        .Select(x => _fileManager.ChangePcPathToWeb(Path.Combine(relativeFolder, Path.GetFileName(x)))).ToList();
    }

    public List<string> GetStandardDefaultUrls(DefaultSizeType size, string pattern)
    {
      var relativeFolder = "";

      switch (size)
      {
        case DefaultSizeType.Original:
          relativeFolder = _folderConfiguration.Defaults.Originals.GetFullHashPath();
          break;
        case DefaultSizeType.Small:
          relativeFolder = _folderConfiguration.Defaults.Smalls.GetFullHashPath();
          break;
        case DefaultSizeType.Large:
          relativeFolder = _folderConfiguration.Defaults.Larges.GetFullHashPath();
          break;
      }

      var pcPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), relativeFolder);

      if (string.IsNullOrEmpty(pattern))
        return Directory.GetFiles(pcPath)
          .Select(x => _fileManager.ChangePcPathToWeb(Path.Combine(relativeFolder, Path.GetFileName(x)))).ToList();
      return Directory.GetFiles(pcPath).Where(x => x.Contains(pattern))
        .Select(x => _fileManager.ChangePcPathToWeb(Path.Combine(relativeFolder, Path.GetFileName(x)))).ToList();
    }

    public List<SizedUrl<AvatarSizeType>> UploadAvatar(string url, AvatarType avatarType)
    {
      return UploadAvatar(url, avatarType, x => new WebClient { Headers = new WebHeaderCollection { "User-Agent: Other" } }.DownloadFile(new Uri(url), x));
    }

    private List<SizedUrl<AvatarSizeType>> UploadAvatar(string fileName, AvatarType avatarType,
      Action<string> saveMethod)
    {
      var imageName = _fileManager.CreateUniqueName(fileName);
      var originalImagePath = CreateRelativeAvatarPath(imageName, avatarType, AvatarSizeType.Original);
      var originalFilePath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), originalImagePath);

      saveMethod.Invoke(originalFilePath);

      var smallImagePath = CreateRelativeAvatarPath(imageName, avatarType, AvatarSizeType.Small);
      var largemagePath = CreateRelativeAvatarPath(imageName, avatarType, AvatarSizeType.Large);

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
          Size = AvatarSizeType.Small,
          Url = _fileManager.ChangePcPathToWeb(smallImagePath)
        },
        new SizedUrl<AvatarSizeType> {Size = AvatarSizeType.Large, Url = _fileManager.ChangePcPathToWeb(largemagePath)}
      };

      return result;
    }

    public List<SizedUrl<BackgroundSizeType>> UploadBackground(string url, BackgroundType backgroundType)
    {
      return UploadBackground(url, backgroundType, x => new WebClient { Headers = new WebHeaderCollection { "User-Agent: Other" } }.DownloadFile(new Uri(url), x));
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
        case AvatarSizeType.Small:
          relativeFolder = avatarTypeFolder.Smalls.GetFullHashPath();
          break;
        case AvatarSizeType.Large:
          relativeFolder = avatarTypeFolder.Larges.GetFullHashPath();
          break;
      }

      return Path.Combine(relativeFolder, name + $"_{size.ToString().ToLower()}" + ext);
    }

    public string CreateRelativeDefaulsPath(string imageName, DefaultSizeType size)
    {
      var name = Path.GetFileNameWithoutExtension(imageName);
      var ext = Path.GetExtension(imageName);
      var relativeFolder = "";
      switch (size)
      {
        case DefaultSizeType.Original:
          relativeFolder = _folderConfiguration.Defaults.Originals.GetFullHashPath();
          break;
        case DefaultSizeType.Small:
          relativeFolder = _folderConfiguration.Defaults.Smalls.GetFullHashPath();
          break;
        case DefaultSizeType.Large:
          relativeFolder = _folderConfiguration.Defaults.Larges.GetFullHashPath();
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

    public bool IsAlreadyDownloaded(string url)
    {
      var uri = new Uri(url);
      return uri.Host.Contains("media.2buttons.ru");
    }

    public void CopyBackgrounds(string sourceFolder, BackgroundType backgroundType, BackgroundSizeType copyToNewSize)
    {
      if (string.IsNullOrEmpty(sourceFolder)) throw new Exception("SourceUrl is null or empty");
      var fullSourcePath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), sourceFolder);
      var fullNewPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(),
        CreateBackgroundPath(backgroundType, copyToNewSize));

      Size size;
      switch (copyToNewSize)
      {
        case BackgroundSizeType.Original:
          size = null;
          break;
        case BackgroundSizeType.Mobile:
          size = new MobileBackgroundSizeFolder(null).Size;
          break;
        default: throw new Exception("Such BackgrodunSizeType does not exist.");
      }

      var files = Directory.GetFiles(fullSourcePath);
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file);
        var ext = Path.GetExtension(file);

        if (fileName.Contains("original")) fileName = fileName.Replace("original", copyToNewSize.ToString().ToLower());
        else
          fileName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + "_" +
                     copyToNewSize.ToString().ToLower();

        var fullNewFilePath = Path.Combine(fullNewPath, fileName + ext);

        if (!File.Exists(fullNewFilePath))
          if (size == null) CopyFile(file, fullNewFilePath);
          else ResizeImage(file, fullNewFilePath, size.Height, size.Width);
      }
    }

    private string CreateBackgroundPath(BackgroundType backgroundType, BackgroundSizeType copyToNewSize)
    {
      return Path.Combine(GetHashFolderName("Background"), GetHashFolderName(backgroundType.ToString()),
        GetHashFolderName(copyToNewSize.ToString()));
    }

    public void CopyAvatars(string sourceFolder, AvatarType backgroundType, AvatarSizeType copyToNewSize)
    {
      if (string.IsNullOrEmpty(sourceFolder)) throw new Exception("SourceUrl is null or empty");
      var fullSourcePath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(), sourceFolder);
      var fullNewPath = Path.Combine(_fileManager.GetAbsoluteMediaRootPath(),
        CreateAvatarPath(backgroundType, copyToNewSize));

      Size size;
      switch (copyToNewSize)
      {
        case AvatarSizeType.Original:
          size = null;
          break;
        case AvatarSizeType.Small:
          size = new SmallAvatarSizeFolder(null).Size;
          break;
        case AvatarSizeType.Large:
          size = new LargeAvatarSizeFolder(null).Size;
          break;
        default: throw new Exception("Such AvatarSizeType does not exist.");
      }

      var files = Directory.GetFiles(fullSourcePath);
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file);
        var ext = Path.GetExtension(file);

        if (fileName.Contains("original")) fileName = fileName.Replace("original", copyToNewSize.ToString().ToLower());
        else
          fileName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + "_" +
                     copyToNewSize.ToString().ToLower();
        var fullNewFilePath = Path.Combine(fullNewPath, fileName + ext);
        if (!File.Exists(fullNewFilePath))
          if (size == null) CopyFile(file, fullNewFilePath);
          else ResizeImage(file, fullNewFilePath, size.Height, size.Width);
      }
    }

    private string CreateAvatarPath(AvatarType backgroundType, AvatarSizeType copyToNewSize)
    {
      return Path.Combine(GetHashFolderName("Avatar"), GetHashFolderName(backgroundType.ToString()),
        GetHashFolderName(copyToNewSize.ToString()));
    }

    private string GetHashFolderName(string str)
    {
      return str.Substring(0, 2) + str.GetMd5HashString().Substring(0, 5);
    }

    public void ResizeImage(string originalPath, string rezisePath, int height, int width)
    {
      using (var image = Image.Load(originalPath))
      {
        image.Mutate(x => x.Resize(new ResizeOptions
        {
          Mode = ResizeMode.Max,
          Size = new SixLabors.Primitives.Size {Height = height, Width = width}
        }));
        image.Save(rezisePath); // Automatic encoder selected based on extension.
      }
    }

    public void CopyFile(string originalPath, string targetPath)
    {
      using (var image = Image.Load(originalPath))
      {
        image.Save(targetPath); // Automatic encoder selected based on extension.
      }
    }
  }
}