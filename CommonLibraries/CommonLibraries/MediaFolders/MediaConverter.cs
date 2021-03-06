﻿using System;
using System.IO;
using System.Linq;
using CommonLibraries.MediaFolders.Configurations;
using Microsoft.Extensions.Options;

namespace CommonLibraries.MediaFolders
{
  public class MediaConverter
  {
    private FolderConfiguration FolderConfiguration { get; } = new FolderConfiguration();
    private string MediaUrl { get; }

    public MediaConverter(IOptions<MediaConverterSettings> options)
    {
      MediaUrl = options.Value.MediaConverterUrl;
    }

    public MediaType GetMediaUrlType(string shortOriginalUrl)
    {
      shortOriginalUrl = string.Concat(shortOriginalUrl.SkipWhile(x => x == '/'));
      var fragments = shortOriginalUrl.Split('/');
      if (fragments[0] == FolderConfiguration.Backgrounds.HashName) return MediaType.Background;
      if (fragments[0] == FolderConfiguration.Avatars.HashName) return MediaType.Avatar;
      if (fragments[0] == FolderConfiguration.Defaults.HashName) return MediaType.Default;
      return MediaType.None;
    }

    public bool IsStandardBackground(string shortOriginalUrl)
    {
      return shortOriginalUrl.Contains(new StandardBackgroundFolder(null).HashName);
    }

    public string ToFullBackgroundurlUrl(string shortOriginalUrl, BackgroundSizeType backgroundSize)
    {
      if (string.IsNullOrEmpty(shortOriginalUrl)) return null;
      if (shortOriginalUrl.Contains(MediaUrl)) shortOriginalUrl = shortOriginalUrl.Replace(MediaUrl, "");
      var name = Path.GetFileName(shortOriginalUrl);
      shortOriginalUrl = Path.GetDirectoryName(string.Concat(shortOriginalUrl.SkipWhile(x => x == '/')));

      if (backgroundSize == BackgroundSizeType.Original)
        return MediaUrl + shortOriginalUrl.Replace("\\", "/") + '/' + name;

      name = name.Replace("original", backgroundSize.ToString().ToLower());

      switch (backgroundSize)
      {
        case BackgroundSizeType.Mobile:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/").Replace(new OriginalSizeFolder(null).HashName,
                   new MobileBackgroundSizeFolder(null).HashName) + '/' + name;
        default: throw new Exception($"There is no such background size {backgroundSize}");
      }
    }

    public string ToFullAvatarUrl(string shortOriginalUrl, AvatarSizeType avatarSize)
    {
      if (string.IsNullOrEmpty(shortOriginalUrl)) return null;
      if (shortOriginalUrl.Contains(MediaUrl)) shortOriginalUrl = shortOriginalUrl.Replace(MediaUrl, "");
      var name = Path.GetFileName(shortOriginalUrl);
      shortOriginalUrl = Path.GetDirectoryName(string.Concat(shortOriginalUrl.SkipWhile(x => x == '/')));

      if (avatarSize == AvatarSizeType.Original) return MediaUrl + shortOriginalUrl.Replace("\\", "/") + '/' + name;

      name = name.Replace("original", avatarSize.ToString().ToLower());

      switch (avatarSize)
      {
        case AvatarSizeType.Small:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/")
                   .Replace(new OriginalSizeFolder(null).HashName, new SmallAvatarSizeFolder(null).HashName) + '/' +
                 name;

        case AvatarSizeType.Large:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/")
                   .Replace(new OriginalSizeFolder(null).HashName, new LargeAvatarSizeFolder(null).HashName) + '/' +
                 name;

        default: throw new Exception($"There is no such avatar size {avatarSize}");
      }
    }

    public string ToFullDefaultUrl(string shortOriginalUrl, DefaultSizeType defaultSize)
    {
      if (string.IsNullOrEmpty(shortOriginalUrl)) return null;
      if (shortOriginalUrl.Contains(MediaUrl)) shortOriginalUrl = shortOriginalUrl.Replace(MediaUrl, "");
      var name = Path.GetFileName(shortOriginalUrl);
      shortOriginalUrl = Path.GetDirectoryName(string.Concat(shortOriginalUrl.SkipWhile(x => x == '/')));

      if (defaultSize == DefaultSizeType.Original) return MediaUrl + shortOriginalUrl.Replace("\\", "/") + '/' + name;

      name = name.Replace("original", defaultSize.ToString().ToLower());

      switch (defaultSize)
      {
        case DefaultSizeType.Small:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/").Replace(new OriginalSizeFolder(null).HashName,
                   new DefaultFolder.SmallDefaultSizeFolder(null).HashName) + '/' + name;

        case DefaultSizeType.Large:
          return MediaUrl + shortOriginalUrl.Replace("\\", "/").Replace(new OriginalSizeFolder(null).HashName,
                   new DefaultFolder.LargeDefaultSizeFolder(null).HashName) + '/' + name;

        default: throw new Exception($"There is no such default size {defaultSize}");
      }
    }
  }
}