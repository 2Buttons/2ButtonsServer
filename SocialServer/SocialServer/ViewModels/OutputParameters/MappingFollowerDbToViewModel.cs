using System.Collections.Generic;
using System.Linq;
using CommonLibraries.Extensions;
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
          SmallAvatarLink = f.SmallAvatarLink,
          Age = f.BirthDate.Age(),
          SexType = f.Sex,
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
          SmallAvatarLink = f.SmallAvatarLink,
          Age = f.BirthDate.Age(),
          SexType = f.Sex,
          VisitsAmount = f.Visits,
          IsYouFollowed = f.YouFollowed,
          IsHeFollowed = f.HeFollowed
        })
        .ToList();
    }
  }
}