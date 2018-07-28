using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediaServer.Infrastructure.Services
{
  public class FileService
  {
    private readonly FolderConfiguration _folderConfiguration;
    private readonly ILogger<FileService> _logger;
    public MediaSettings MediaOptions { get; }
    public IHostingEnvironment AppEnvironment { get; }

    public FileService(IHostingEnvironment appEnvironment, IOptions<MediaSettings> mediaOptions,
      FolderConfiguration folderConfiguration, ILogger<FileService> logger)
    {
      MediaOptions = mediaOptions.Value;
      AppEnvironment = appEnvironment;
      _folderConfiguration = folderConfiguration;
      _logger = logger;
      InitConfiguration();
    }

    public bool IsUrlValid(string url)
    {
      var paths = new List<string>
      {
        AppContext.BaseDirectory,
        MediaOptions.RootFolderRelativePath,
        MediaOptions.RootFolderName
      };
      paths.AddRange(url.Trim().Split('/'));

      var path = Path.Combine(paths.ToArray());
      return File.Exists(path);
    }

    public string CreateServerPath(string imageType, string imageName)
    {
      return Path.Combine(GetAbsoluteMediaRootPath(), imageType, imageName);
    }

    public string GetWebPath(string imageType, string imageName)
    {
      return "/" + imageType + "/" + imageName;
    }

    public string ChangePcPathToWeb(string pcPath)
    {
      return pcPath.Replace('\\', '/');
    }

    public void InitConfiguration()
    {
      CreateFolders(_folderConfiguration, GetAbsoluteMediaRootPath());
    }

    public string GetAbsoluteMediaRootPath()
    {
      return Path.Combine(AppContext.BaseDirectory, MediaOptions.RootFolderRelativePath + MediaOptions.RootFolderName,
        "");
    }

    public string GetAbsoluteMediaFolderPath(string folderName)
    {
      return Path.Combine(GetAbsoluteMediaRootPath(), folderName);
    }

    public void CreateFolder(BaseFolder folder, string mediaFolder)
    {
      var path = Path.Combine(mediaFolder, folder.GetFullHashPath());
      Directory.CreateDirectory(path);
    }

    public void CreateFolders(FolderConfiguration config, string mediaFolder)
    {
      _logger.LogInformation($"{nameof(FileService)}.{nameof(CreateFolders)}.Start");
      IList<string> paths = new List<string>();
      var type = config.GetType();

      foreach (var property in type.GetProperties()
        .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(FolderAttribute))).ToList())
      {
        var t = property.GetValue(config);
        var baseFolder = (BaseFolder) property.GetValue(config);
        GetSubFolder(baseFolder, paths);
      }
      foreach (var path in paths) Directory.CreateDirectory(Path.Combine(mediaFolder, path));
      _logger.LogInformation($"{nameof(FileService)}.{nameof(CreateFolders)}.End");
    }

    public void GetSubFolder(BaseFolder folder, IList<string> paths)
    {
      _logger.LogInformation($"{nameof(FileService)}.{nameof(GetSubFolder)}.Start");
      paths.Add(folder.GetFullHashPath());
      var type = folder.GetType();
      var properties = type.GetProperties()
        .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(FolderAttribute))).ToList();
      foreach (var property in properties) GetSubFolder((BaseFolder) property.GetValue(folder), paths);
      _logger.LogInformation($"{nameof(FileService)}.{nameof(GetSubFolder)}.End");
    }

    public string CreateUniqueName(string imageName)
    {
      var ext = Path.GetExtension(imageName);
      if (ext.IsNullOrEmpty())
        ext = imageName.ToLower().Contains(".png") ? ".png" : ".jpg";
      else
        ext = ext.Substring(0, 4);
      if (ext == ".jpe") ext = ".jpg";
      return imageName.GetMd5HashString().Substring(0, 2) +
             Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + ext;
    }
  }
}