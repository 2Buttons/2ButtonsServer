using System.Collections.Generic;
using System.Linq;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using SocialData.Main.DTO;
using SocialData.Main.Queries;

namespace SocialServer.ViewModels.OutputParameters
{
  public static class MappingFollowerDbToViewModel
  {

    public static List<GetFollowingViewModel> MapToUserContactsViewModel(this IEnumerable<FollowingQuery> userContactsDb, MediaConverter mediaConverter)
    {
      return userContactsDb.Select(f => new GetFollowingViewModel
      {
          UserId = f.UserId,
          Login = f.FirstName + " " + f.LastName,
          SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(f.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
          Age = f.BirthDate.Age(),
          SexType = f.SexType,
          VisitsCount = f.VisitsCount,
          IsYouFollowed = f.IsHeFollowed,
          IsHeFollowed = f.IsYouFollowed
      })
        .ToList();
    }

    //public static List<GetFollowerViewModel> MapToUserContactsViewModel(this IEnumerable<FollowerDto> userContactsDb, MediaConverter mediaConverter)
    //{
    //  return userContactsDb.Select(f => new GetFollowerViewModel
    //    {
    //      UserId = f.UserId,
    //      Login = f.Login,
    //      SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(f.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
    //      Age = f.BirthDate.Age(),
    //      SexType = f.SexType,
    //      IsYouFollowed = f.IsYouFollowed,
    //      IsHeFollowed = f.IsHeFollowed
    //    })
    //    .ToList();
    //}
  }
}