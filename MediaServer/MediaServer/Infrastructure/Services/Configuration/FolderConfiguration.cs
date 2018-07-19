using System;
using System.Collections.Generic;
using System.IO;

namespace MediaServer.Infrastructure.Services.Configuration
{
  public class FolderConfiguration
  {
    [Folder]
    public BackgroundFolder Backgrounds { get; set; }

    [Folder]
    public AvatarFolder Avatars { get; set; }

    [Folder]
    public DefaultFolder Defaults { get; set; }

    public FolderConfiguration()
    {
      Backgrounds = new BackgroundFolder(null);
      Avatars = new AvatarFolder(null);
      Defaults = new DefaultFolder(null);
    }
  }

  public class OriginalSizeFolder : SizeFolder
  {
    public OriginalSizeFolder(BaseFolder rootFolder) : base("Original", rootFolder)
    {
    }
  }

  public abstract class BaseFolder
  {
    public BaseFolder RootFolder { get; set; }
    public string FolderName { get; set; }
    public string HashName => FolderName.Substring(0, 2) + FolderName.GetMd5Hash().Substring(0, 4);

    public BaseFolder(string folderName, BaseFolder rootFolder)
    {
      FolderName = folderName;
      RootFolder = rootFolder;
    }

    public string GetFullPath()
    {
      var paths = new List<string>();

      var currentFolder = this;
      while (currentFolder != null)
      {

        paths.Add(currentFolder.FolderName);
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

        paths.Add(currentFolder.FolderName.Substring(0, 2) + currentFolder.FolderName.GetMd5Hash().Substring(0, 5));
        currentFolder = currentFolder.RootFolder;
      }
      paths.Reverse();
      var result = Path.Combine(paths.ToArray());
      return result;
    }
  }

  public abstract class SizeFolder : BaseFolder
  {
    public Size Size { get; set; }

    public SizeFolder(string folderName, BaseFolder rootFolder) : base(folderName, rootFolder)
    {
    }

    
  }

  public class Size
  {
    public int Height { get; set; }
    public int Width { get; set; }
  }

  public class FolderAttribute : Attribute
  {
  }
}