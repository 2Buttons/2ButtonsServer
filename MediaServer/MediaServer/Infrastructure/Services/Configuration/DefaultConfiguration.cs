using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServer.Infrastructure.Services.Configuration
{
  public class DefaultFolder : BaseFolder
  {
    [Folder]
    public SmallAvatarSizeFolder Smalls { get; set; }
    [Folder]
    public LargeAvatarSizeFolder Larges { get; set; }
    [Folder]
    public OriginalSizeFolder Originals { get; set; }

    public DefaultFolder(BaseFolder rootFolder) : base("Default", rootFolder)
    {
      Smalls = new SmallAvatarSizeFolder(this);
      Larges = new LargeAvatarSizeFolder(this);
      Originals = new OriginalSizeFolder(this);
    }
  }
}
