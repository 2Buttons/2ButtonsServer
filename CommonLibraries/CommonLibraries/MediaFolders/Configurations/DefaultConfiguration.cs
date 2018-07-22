namespace CommonLibraries.MediaFolders.Configurations
{
  public class DefaultFolder : BaseFolder
  {
    [Folder]
    public SmallDefaultSizeFolder Smalls { get; set; }
    [Folder]
    public LargeDefaultSizeFolder Larges { get; set; }
    [Folder]
    public OriginalSizeFolder Originals { get; set; }

    public DefaultFolder(BaseFolder rootFolder) : base("Default", rootFolder)
    {
      Smalls = new SmallDefaultSizeFolder(this);
      Larges = new LargeDefaultSizeFolder(this);
      Originals = new OriginalSizeFolder(this);
    }


    public class SmallDefaultSizeFolder : SizeFolder
    {
      public SmallDefaultSizeFolder(BaseFolder rootFolder) : base("Small", rootFolder)
      {
        Size = new Size { Height = 100, Width = 100 };
      }
    }

    public class LargeDefaultSizeFolder : SizeFolder
    {
      public LargeDefaultSizeFolder(BaseFolder rootFolder) : base("Large", rootFolder)
      {
        Size = new Size { Height = 600, Width = 600 };
      }
    }
  }
}
