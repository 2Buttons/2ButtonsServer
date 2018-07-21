using System.Collections.Generic;
using System.Linq;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using SocialData.Main.Entities.Followers;

namespace SocialServer.ViewModels.OutputParameters
{
  public static class MappingFollowerDbToViewModel
  {
    public static List<GetFollowerViewModel> MapToUserContactsViewModel(this IEnumerable<FollowerDb> userContactsDb)
    {
      return userContactsDb.Select(f => new GetFollowerViewModel
        {
          UserId = f.UserId,
          Login = f.Login,
          SmallAvatarLink = MediaConverter.ToFullAvatarUrl(f.OriginalAvatarLink, CommonLibraries.AvatarSizeType.Small),
          Age = f.BirthDate.Age(),
          SexType = f.SexType,
          IsYouFollowed = f.YouFollowed,
          IsHeFollowed = f.HeFollowed
        })
        .ToList();
    }

    public static List<GetFollowToViewModel> MapToUserContactsViewModel(this IEnumerable<FollowToDb> userContactsDb)
    {
      return userContactsDb.Select(f => new GetFollowToViewModel
        {
          UserId = f.UserId,
          Login = f.Login,
          SmallAvatarLink = MediaConverter.ToFullAvatarUrl(f.OriginalAvatarLink, CommonLibraries.AvatarSizeType.Small),
          Age = f.BirthDate.Age(),
          SexType = f.SexType,
          VisitsAmount = f.Visits,
          IsYouFollowed = f.YouFollowed,
          IsHeFollowed = f.HeFollowed
        })
        .ToList();
    }
  }
}