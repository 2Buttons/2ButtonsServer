using System.Collections.Generic;
using System.Linq;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using SocialData.Main.Entities.Followers;

namespace SocialServer.ViewModels.OutputParameters
{
  public static class MappingFollowerDbToViewModel
  {
    public static List<GetFollowerViewModel> MapToUserContactsViewModel(this IEnumerable<FollowerDb> userContactsDb, MediaConverter mediaConverter)
    {
      return userContactsDb.Select(f => new GetFollowerViewModel
        {
          UserId = f.UserId,
          Login = f.Login,
          SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(f.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
          Age = f.BirthDate.Age(),
          SexType = f.SexType,
          IsYouFollowed = f.IsYouFollowed,
          IsHeFollowed = f.IsHeFollowed
        })
        .ToList();
    }

    public static List<GetFollowToViewModel> MapToUserContactsViewModel(this IEnumerable<FollowToDb> userContactsDb, MediaConverter mediaConverter)
    {
      return userContactsDb.Select(f => new GetFollowToViewModel
        {
          UserId = f.UserId,
          Login = f.Login,
          SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(f.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
          Age = f.BirthDate.Age(),
          SexType = f.SexType,
          VisitsCount = f.VisitsCount,
          IsYouFollowed = f.IsYouFollowed,
          IsHeFollowed = f.IsHeFollowed
        })
        .ToList();
    }
  }
}