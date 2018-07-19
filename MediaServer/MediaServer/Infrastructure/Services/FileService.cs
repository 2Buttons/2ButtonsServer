using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace MediaServer.Infrastructure.Services
{
  public class FileService
  {
   // private readonly IDictionary<string, string> _folders;

 
    private readonly FolderConfiguration _folderConfiguration;

    public FileService(IHostingEnvironment appEnvironment, IOptions<MediaSettings> mediaOptions, FolderConfiguration folderConfiguration)
    {
      MediaOptions = mediaOptions.Value;
      AppEnvironment = appEnvironment;
     // _folders = new Dictionary<string, string>();
      _folderConfiguration = folderConfiguration;
      InitConfiguration();
    }

    public MediaSettings MediaOptions { get; }
    public IHostingEnvironment AppEnvironment { get; }

    public bool IsUrlValid(string url)
    {
      //url = "/" + url;
      //var pattern = @"/+";
      //var target = "/";
      //var result = new Regex(pattern).Replace(url, target);
      //var counts = result.Count(x => x == '/');
      //if (counts >= 3 || counts < 2) return false;
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

    //public List<string> GetMediaFolders()
    //{
    //  return _folders.Values.ToList();
    //}

    //public bool IsValidImageType(string imageType)
    //{
    //  return _folders.Keys.Contains(imageType);
    //}

    //public bool TryConvertHashIntoImageType(string hashCode, out string imageType)
    //{
    //  return _folders.TryGetValue(hashCode, out imageType);
    //}

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
      return pcPath.Replace('\\','/');
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

    public string CreateUniqueOriginalName(string imageName)
    {
      var ext = Path.GetExtension(imageName).Substring(0, 4);
      if (ext.IsNullOrEmpty()) ext = ".jpg";
      return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + "_original" + ext;
    }

    public void CreateFolder(BaseFolder folder, string mediaFolder)
    {
      var path = Path.Combine(mediaFolder, folder.GetFullHashPath());
      Directory.CreateDirectory(path);
    }

    public void CreateFolders(FolderConfiguration config, string mediaFolder)
    {
      IList<string> paths = new List<string>();
      var type = config.GetType();

      //var pp = type.GetProperties();
      //var attr  = type.GetProperties().Select(x => x.CustomAttributes).ToList();
      //var super = type.GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(FolderAttribute))).ToList();
      foreach (var property in type.GetProperties().Where(x=>x.CustomAttributes.Any(y=>y.AttributeType==typeof(FolderAttribute))).ToList())
      {
        var t = property.GetValue(config);
        var baseFolder = (BaseFolder) property.GetValue(config);
        GetSubFolder(baseFolder, paths);
      }
      foreach (var path in paths)
      {
        Directory.CreateDirectory(Path.Combine(mediaFolder, path));
      }
    }

    public void GetSubFolder(BaseFolder folder, IList<string> paths)
    {
      paths.Add(folder.GetFullHashPath());
      var type = folder.GetType();
      var properties = type.GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(FolderAttribute))).ToList();
      foreach (var property in properties)
      {

        GetSubFolder((BaseFolder)property.GetValue(folder), paths);

      }
    }

    //public void CreateSampleFiles(FolderConfiguration f, string mediaFolder)
    //{
    //  foreach (var folder in folders)
    //  {
    //    var path = Path.Combine(mediaFolder, folder.GetFullHashPath(), folder.Name + ".txt");
    //    //PATHS.Add(path);
    //    File.Create(path);

    //    if (folder.Subfolders != null && folder.Subfolders.Any()) CreateSampleFiles(folder.Subfolders, mediaFolder);
    //  }
    //}

    public void CreateSampleFiles(IEnumerable<Folder> folders, string mediaFolder)
    {
      foreach (var folder in folders)
      {
        var path = Path.Combine(mediaFolder, folder.GetFullHashPath(), folder.Name + ".txt");
        //PATHS.Add(path);
        File.Create(path);

        if (folder.Subfolders != null && folder.Subfolders.Any()) CreateSampleFiles(folder.Subfolders, mediaFolder);
      }
    }

    private void CreateIfNotExistsRootFolder()
    {
      Directory.CreateDirectory(GetAbsoluteMediaRootPath());
    }

    private void CreateIfNotExistsMediaFolder(string folderName)
    {
      Directory.CreateDirectory(GetAbsoluteMediaFolderPath(folderName));
    }

    public void CreateFolders(IEnumerable<Folder> folders, string mediaFolder)
    {
      foreach (var folder in folders)
      {
        var path = Path.Combine(mediaFolder, folder.GetFullHashPath());
        Directory.CreateDirectory(path);
        //PATHS.Add(path);

        if (folder.Subfolders != null && folder.Subfolders.Any()) CreateFolders(folder.Subfolders, mediaFolder);
      }
    }

    public string CreateUniqueName(string imageName)
    {
      var ext = Path.GetExtension(imageName).Substring(0, 4);
      if (ext.IsNullOrEmpty()) ext = ".jpg";
      return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + ext;
    }
  }

  public class Folder
  {
    public string Name { get; set; }
    public Folder RootFolder { get; set; }
    public IEnumerable<Folder> Subfolders { get; set; }

    public Folder()
    {
    }

    public Folder(string name)
    {
      Name = name;
    }

    public string GetFullPath()
    {
      var paths = new List<string>();

      var currentFolder = this;
      while (currentFolder != null)
      {
        paths.Add(currentFolder.Name);
        currentFolder = currentFolder.RootFolder;
      }
      paths.Reverse();
      var result = Path.Combine(paths.ToArray());
      return result;
    }

    public string GetFullHashPath()
    {
      var paths = new List<string>();

      var currentFolder = this;
      while (currentFolder != null)
      {
        paths.Add(currentFolder.Name.Substring(0, 2) + currentFolder.Name.GetMd5HashString().Substring(0, 5));
        currentFolder = currentFolder.RootFolder;
      }
      paths.Reverse();
      var result = Path.Combine(paths.ToArray());
      return result;
    }
  }
}