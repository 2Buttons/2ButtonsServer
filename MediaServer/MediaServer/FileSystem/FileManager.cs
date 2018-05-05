using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MediaServer.FileSystem
{
  public class FileManager : IFileManager
  {
    private readonly IDictionary<string, string> _folders;

    public IOptions<MediaData> MediaOptions { get; }
    public IHostingEnvironment AppEnvironment { get; }

    public FileManager(IHostingEnvironment appEnvironment, IOptions<MediaData> mediaOptions)
    {
      MediaOptions = mediaOptions;
      AppEnvironment = appEnvironment;
      _folders = new Dictionary<string, string>();
      InitConfiguration();
    }

    public bool IsUrlValid(string url)
    {
      var serverUrl = Path.Combine(url.Split("/")[1], url.Split("/")[2]);
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaOptions.Value.RootFolderRelativePath,
        MediaOptions.Value.RootFolderName, serverUrl);
      return File.Exists(path);
    }

    public List<string> GetMediaFolders()
    {
      return _folders.Values.ToList();
    }

    public bool IsValidImageType(string imageType)
    {
      return _folders.Keys.Contains(imageType);
    }

    public bool TryConvertHashIntoImageType(string hashCode, out string imageType)
    {
      return _folders.TryGetValue(hashCode, out imageType);
    }

    public string CreateServerPath(string imageType, string imageName)
    {
      return Path.Combine(GetAbsoluteRootPath(),
          imageType, imageName);
    }

    public string GetWebPath(string imageType, string imageName)
    {
      return "/" + imageType + "/" + imageName;
    }

    public string CreateUniqueName(string imageName)
    {
      return Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageName);
    }

    public void InitConfiguration()
    {

      var folders = new List<string>
      {
        "Background",
        "CustomBackground",
        "UserFullAvatarPhoto",
        "UserSmallAvatarPhoto",
        "DefaultMediaFolder",
      };

      foreach (var folder in folders)
      {
        _folders.Add(folder.GetMd5Hash(), folder);
      }

      CreateIfNotExistsRootFolder();
      foreach (var mediaFolder in _folders.Keys)
        CreateIfNotExistsMediaFolder(mediaFolder);
    }

    private void CreateIfNotExistsRootFolder()
    {
      Directory.CreateDirectory(GetAbsoluteRootPath());
    }

    private void CreateIfNotExistsMediaFolder(string folderName)
    {
      Directory.CreateDirectory(GetAbsoluteMediaFolderPath(folderName));
    }

    private string GetAbsoluteRootPath()
    {
      return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaOptions.Value.RootFolderRelativePath + MediaOptions.Value.RootFolderName, "");
    }

    private string GetAbsoluteMediaFolderPath(string folderName)
    {
      return Path.Combine(GetAbsoluteRootPath(), folderName);
    }
  }
}