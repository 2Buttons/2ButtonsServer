using System.IO;
using System.Linq;
using CommonLibraries.MediaFolders.Configurations;

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

    public static string ToFullBackgroundurlUrl(string shortOriginalUrl, BackgroundSizeType backgroundSize)
    {
      var relativePath = "";
      var name = Path.GetFileName(shortOriginalUrl);
      name = name.Replace("original", backgroundSize.ToString().ToLower());

      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));
      var fragments = shortOriginalUrl.Split('/');
      if (fragments[1] == FolderConfiguration.Backgrounds.Customs.HashName)
        switch (backgroundSize)
        {
          case BackgroundSizeType.Original:
            relativePath = FolderConfiguration.Backgrounds.Customs.Originals.GetFullHashPath();
            break;
          case BackgroundSizeType.Mobile:
            relativePath = FolderConfiguration.Backgrounds.Customs.Mobiles.GetFullHashPath();
            break;
        }
      else if (fragments[1] == FolderConfiguration.Backgrounds.Standards.HashName)
        switch (backgroundSize)
        {
          case BackgroundSizeType.Original:
            relativePath = FolderConfiguration.Backgrounds.Standards.Originals.GetFullHashPath();
            break;
          case BackgroundSizeType.Mobile:
            relativePath = FolderConfiguration.Backgrounds.Standards.Mobiles.GetFullHashPath();
            break;
        }

      return MediaUrl + relativePath.Replace("\\", "/") + name;
    }

    public static string ToFullAvatarUrl(string shortOriginalUrl, AvatarSizeType  avatarSize)
    {
      var relativePath = "";
      var name = Path.GetFileName(shortOriginalUrl);
      name = name.Replace("original", avatarSize.ToString().ToLower());

      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));
      var fragments = shortOriginalUrl.Split('/');
      if (fragments[1] == FolderConfiguration.Backgrounds.Customs.HashName)
        switch (avatarSize)
        {
          case AvatarSizeType.Original:
            relativePath = FolderConfiguration.Avatars.Customs.Originals.GetFullHashPath();
            break;
          case AvatarSizeType.Small:
            relativePath = FolderConfiguration.Avatars.Customs.Smalls.GetFullHashPath();
            break;
          case AvatarSizeType.Large:
            relativePath = FolderConfiguration.Avatars.Customs.Larges.GetFullHashPath();
            break;
        }
      else if (fragments[1] == FolderConfiguration.Backgrounds.Standards.HashName)
        switch (avatarSize)
        {
          case AvatarSizeType.Original:
            relativePath = FolderConfiguration.Avatars.Customs.Originals.GetFullHashPath();
            break;
          case AvatarSizeType.Small:
            relativePath = FolderConfiguration.Avatars.Customs.Smalls.GetFullHashPath();
            break;
          case AvatarSizeType.Large:
            relativePath = FolderConfiguration.Avatars.Customs.Larges.GetFullHashPath();
            break;
        }

      return MediaUrl + relativePath.Replace("\\", "/") + name;
    }

    public static string ToFullDefaultUrl(string shortOriginalUrl, DefaultSizeType defaultSize)
    {
      var relativePath = "";
      var name = Path.GetFileName(shortOriginalUrl);
      name = name.Replace("original", defaultSize.ToString().ToLower());

      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));

      switch (defaultSize)
      {
        case DefaultSizeType.Original:
          relativePath = FolderConfiguration.Defaults.Originals.GetFullHashPath();
          break;
        case DefaultSizeType.Small:
          relativePath = FolderConfiguration.Defaults.Smalls.GetFullHashPath();
          break;
        case DefaultSizeType.Large:
          relativePath = FolderConfiguration.Defaults.Larges.GetFullHashPath();
          break;
      }

      return MediaUrl + relativePath.Replace("\\", "/") + name;
    }
  }
}