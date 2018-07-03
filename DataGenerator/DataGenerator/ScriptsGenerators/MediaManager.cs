using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CommonLibraries;
using CommonLibraries.Extensions;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class MediaManager
  {
    public void SetStandardsBackground(IList<QuestionEntity> questions, string standardsBackgroundFolder)
    {
      var random = new Random();
      var backgrounds = Directory.GetFiles(standardsBackgroundFolder).Where(x => x.Contains("background")).ToList();

      foreach (var t in questions)
      {
        var backgroundIndex = random.Next(1, backgrounds.Count);
        t.BackgroundImageLink = "/standards/" + backgrounds[backgroundIndex];
      }
    }

    public void SetCustomBackground(IList<QuestionEntity> questions, string backgroundFolder, string saveToFolder)
    {
      var random = new Random();
      var backgrounds = Directory.GetFiles(backgroundFolder).ToList();
      var imageType = BackgroundType.Background.ToString().GetMd5Hash();

      foreach (var t in questions)
      {
        var backgroundIndex = random.Next(1, backgrounds.Count);

        var oldName = backgrounds[backgroundIndex];
        var uniqueName = CreateUniqueName(backgrounds[backgroundIndex]);
        var newName = $"/{imageType}/" + uniqueName;

        t.BackgroundImageLink = newName;
        if(backgroundFolder != saveToFolder && !File.Exists(Path.Combine(saveToFolder, imageType, uniqueName))) File.Move(Path.Combine(backgroundFolder, oldName), Path.Combine(saveToFolder, imageType, uniqueName));
      }
    }

    public void SetAvatars(IList<UserInfoEntity> users, string saveToFolder)
    {
      foreach (var t in users)
      {
        var smallAvatar = AvatarSizeType.SmallAvatar.ToString().GetMd5Hash();
        var largeAvatar = AvatarSizeType.LargeAvatar.ToString().GetMd5Hash();
        var smallName = CreateUniqueName(t.SmallAvatarLink);
        var largeName = CreateUniqueName(t.LargeAvatarLink);

        var smallFilePath = Path.Combine(saveToFolder, smallAvatar, smallName);
        var largeFilePath = Path.Combine(saveToFolder, largeAvatar, largeName);

        new WebClient().DownloadFile(new Uri(t.SmallAvatarLink), smallFilePath);
        new WebClient().DownloadFile(new Uri(t.LargeAvatarLink), largeFilePath);

        t.SmallAvatarLink = "/" + smallAvatar + "/" + smallName;
        t.LargeAvatarLink = "/" + largeAvatar + "/" + largeName;
      }
    }

    private string CreateUniqueName(string imageName)
    {
      var ext = Path.GetExtension(imageName).Substring(0, 4);
      if (ext.IsNullOrEmpty()) ext = ".jpg";
      return Guid.NewGuid().ToString().Replace("-", "") + ext;
    }
  }
}