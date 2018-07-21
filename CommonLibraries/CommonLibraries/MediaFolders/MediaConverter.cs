using System;
using System.IO;
using System.Linq;
using CommonLibraries.MediaFolders.Configurations;
using CommonTypes;

namespace CommonLibraries.MediaFolders
{
  public class MediaConverter
  {
    private static readonly FolderConfiguration FolderConfiguration = new FolderConfiguration();
    public static string MediaUrl { get; } = "http://media.2buttons.ru/";

    public static MediaType GetMediaUrlType(string shortOriginalUrl)
    {
      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));
      var fragments = shortOriginalUrl.Split('/');
      if (fragments[0] == FolderConfiguration.Backgrounds.HashName) return MediaType.Background;
      if (fragments[0] == FolderConfiguration.Avatars.HashName) return MediaType.Avatar;
      if (fragments[0] == FolderConfiguration.Defaults.HashName) return MediaType.Default;
      return MediaType.None;
    }

    public static bool IsStandardBackground(string shortOriginalUrl)
    {
      return shortOriginalUrl.Contains(new StandardBackgroundFolder(null).HashName);
    }

    public static string ToFullBackgroundurlUrl(string shortOriginalUrl, BackgroundSizeType backgroundSize)
    {
      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));

      var name = Path.GetFileName(shortOriginalUrl);

      if (backgroundSize == BackgroundSizeType.Original) return MediaUrl + shortOriginalUrl.Replace("\\", "/") + name;

      name = name.Replace("original", backgroundSize.ToString().ToLower());

      switch (backgroundSize)
      {
        case BackgroundSizeType.Mobile:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/").Replace(new OriginalSizeFolder(null).HashName,
                   new MobileBackgroundSizeFolder(null).HashName) + name;
        default: throw new Exception($"There is no such background size {backgroundSize}");
      }
    }

    public static string ToFullAvatarUrl(string shortOriginalUrl, AvatarSizeType avatarSize)
    {
      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));

      var name = Path.GetFileName(shortOriginalUrl);

      if (avatarSize == AvatarSizeType.Original) return MediaUrl + shortOriginalUrl.Replace("\\", "/") + name;

      name = name.Replace("original", avatarSize.ToString().ToLower());

      switch (avatarSize)
      {
        case AvatarSizeType.Small:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/")
                   .Replace(new OriginalSizeFolder(null).HashName, new SmallAvatarSizeFolder(null).HashName) + name;

        case AvatarSizeType.Large:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/")
                   .Replace(new OriginalSizeFolder(null).HashName, new LargeAvatarSizeFolder(null).HashName) + name;

        default: throw new Exception($"There is no such background size {avatarSize}");
      }
    }

    public static string ToFullDefaultUrl(string shortOriginalUrl, DefaultSizeType defaultSize)
    {
      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));

      var name = Path.GetFileName(shortOriginalUrl);


      name = name.Replace("original", defaultSize.ToString().ToLower());
      return MediaUrl + shortOriginalUrl.Replace("\\", "/") + name;

    }
  }
}