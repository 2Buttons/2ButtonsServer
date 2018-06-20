using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace MediaServer.Infrastructure.Services
{
  public class FileService : IFileService
  {
    private readonly IDictionary<string, string> _folders;

    public MediaData MediaOptions { get; }
    public IHostingEnvironment AppEnvironment { get; }

    public FileService(IHostingEnvironment appEnvironment, IOptions<MediaData> mediaOptions)
    {
      MediaOptions = mediaOptions.Value;
      AppEnvironment = appEnvironment;
      _folders = new Dictionary<string, string>();
      InitConfiguration();
    }

    public bool IsUrlValid(string url)
    {
      url = "/" + url;
      var pattern = @"/+";
      var target = "/";
      var result = new Regex(pattern).Replace(url, target);
      var counts = result.Count(x => x == '/');
      if (counts >= 3 || counts < 2)
        return false;
      var paths = new List<string>
      {
        AppContext.BaseDirectory,
        MediaOptions.RootFolderRelativePath,
        MediaOptions.RootFolderName
      };
      paths.AddRange(result.Split('/'));
     
      var path = Path.Combine(paths.ToArray());
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
      return Path.Combine(GetAbsoluteMediaRootPath(),
          imageType, imageName);
    }

    public string GetWebPath(string imageType, string imageName)
    {
      return "/" + imageType + "/" + imageName;
    }

    public string CreateUniqueName(string imageName)
    {
      var ext = Path.GetExtension(imageName).Substring(0, 4);
      if (ext.IsNullOrEmpty()) ext = ".jpg";
      return Guid.NewGuid().ToString().Replace("-", "") + ext;
    }

    public void InitConfiguration()
    {

      var folders = new List<string>
      {
        "Background",
        "CustomBackground",
        "LargeAvatar",
        "SmallAvatar",
        "DefaultFolder",
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
      Directory.CreateDirectory(GetAbsoluteMediaRootPath());
    }

    private void CreateIfNotExistsMediaFolder(string folderName)
    {
      Directory.CreateDirectory(GetAbsoluteMediaFolderPath(folderName));
    }

    public string GetAbsoluteMediaRootPath()
    {
      return Path.Combine(AppContext.BaseDirectory, MediaOptions.RootFolderRelativePath + MediaOptions.RootFolderName, "");
    }

    public string GetAbsoluteMediaFolderPath(string folderName)
    {
      return Path.Combine(GetAbsoluteMediaRootPath(), folderName);
    }
  }
}