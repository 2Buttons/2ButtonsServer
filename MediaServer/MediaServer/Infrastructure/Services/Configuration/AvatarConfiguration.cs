namespace MediaServer.Infrastructure.Services.Configuration
{
  public class AvatarFolder : BaseFolder
  {
    [Folder]
    public CustomAvatarFolder Customs { get; set; }

    [Folder]
    public StandardAvatarFolder Standards { get; set; }

    public AvatarFolder(BaseFolder rootFolder) : base("Avatar", rootFolder)
    {
      Customs = new CustomAvatarFolder(this);
      Standards = new StandardAvatarFolder(this);
    }
  }

  public class CustomAvatarFolder : AvatarSizeFolders
  {
    public CustomAvatarFolder(BaseFolder rootFolder) : base("Custom", rootFolder)
    {
    }
  }

  public class StandardAvatarFolder : AvatarSizeFolders
  {
    public StandardAvatarFolder(BaseFolder rootFolder) : base("Standard", rootFolder)
    {
    }
  }

  public abstract class AvatarSizeFolders : BaseFolder
  {
    [Folder]
    public SmallAvatarSizeFolder Smalls { get; set; }

    [Folder]
    public LargeAvatarSizeFolder Larges { get; set; }

    [Folder]
    public OriginalSizeFolder Originals { get; set; }

    protected AvatarSizeFolders(string folderName, BaseFolder rootFolder) : base(folderName, rootFolder)
    {
      Smalls = new SmallAvatarSizeFolder(this);
      Larges = new LargeAvatarSizeFolder(this);
      Originals = new OriginalSizeFolder(this);
    }
  }

  public class SmallAvatarSizeFolder : SizeFolder
  {
    public SmallAvatarSizeFolder(BaseFolder rootFolder) : base("Small", rootFolder)
    {
      Size = new Size {Height = 100, Width = 100};
    }
  }

  public class LargeAvatarSizeFolder : SizeFolder
  {
    public LargeAvatarSizeFolder(BaseFolder rootFolder) : base("Large", rootFolder)
    {
      Size = new Size {Height = 600, Width = 600};
    }
  }
}