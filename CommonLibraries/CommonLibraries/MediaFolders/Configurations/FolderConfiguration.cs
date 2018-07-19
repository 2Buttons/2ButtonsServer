using System;
using System.Collections.Generic;
using System.IO;
using CommonLibraries.Extensions;

namespace CommonLibraries.MediaFolders.Configurations
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

  public  class BaseFolder
  {
    public BaseFolder RootFolder { get; set; }
    public string FolderName { get; set; }
    public string HashName => FolderName.Substring(0, 2) + FolderName.GetMd5HashString().Substring(0, 5);

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

        paths.Add(currentFolder.FolderName.Substring(0, 2) + currentFolder.HashName);
        currentFolder = currentFolder.RootFolder;
      }
      paths.Reverse();
      var result = Path.Combine(paths.ToArray());
      return result;
    }
  }

  public  class SizeFolder : BaseFolder
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