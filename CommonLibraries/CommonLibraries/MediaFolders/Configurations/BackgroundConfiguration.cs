namespace CommonLibraries.MediaFolders.Configurations
{
  public class BackgroundFolder : BaseFolder
  {
    [Folder]
    public CustomBackgroundFolder Customs { get; set; }

    [Folder]
    public StandardBackgroundFolder Standards { get; set; }

    public BackgroundFolder(BaseFolder rootFolder) : base("Background", rootFolder)
    {
      Customs = new CustomBackgroundFolder(this);
      Standards = new StandardBackgroundFolder(this);
    }
  }

  public class CustomBackgroundFolder : BackgroundSizeFolders
  {
    public CustomBackgroundFolder(BaseFolder rootFolder) : base("Custom", rootFolder)
    {
    }
  }

  public class StandardBackgroundFolder : BackgroundSizeFolders
  {
    public StandardBackgroundFolder(BaseFolder rootFolder) : base("Standard", rootFolder)
    {
    }
  }

  public abstract class BackgroundSizeFolders : BaseFolder
  {
    [Folder]
    public MobileBackgroundSizeFolder Mobiles { get; set; }

    [Folder]
    public OriginalSizeFolder Originals { get; set; }

    protected BackgroundSizeFolders(string folderName, BaseFolder rootFolder) : base(folderName, rootFolder)
    {
      Originals = new OriginalSizeFolder(this);
      Mobiles = new MobileBackgroundSizeFolder(this);
    }
  }

  public class MobileBackgroundSizeFolder : SizeFolder
  {
    public MobileBackgroundSizeFolder(BaseFolder rootFolder) : base("Mobile", rootFolder)
    {
      Size = new Size {Height = 200, Width = 512};
    }
  }
}