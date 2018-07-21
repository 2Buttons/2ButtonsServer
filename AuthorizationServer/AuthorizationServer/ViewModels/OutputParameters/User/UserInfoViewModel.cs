using System;
using AuthorizationData.Main.Entities;
using CommonLibraries.MediaFolders;
using CommonTypes;

namespace AuthorizationServer.ViewModels.OutputParameters.User
{
  public class UserInfoViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string LargetAvatarUrl { get; set; }
    public string SmallAvatarUrl { get; set; }

    //public int UserId { get; set; }
    //public string Login { get; set; }
    //public int Age { get; set; }
    //public SexType SexType { get; set; }
    //public string Description { get; set; }
    //public string SmallAvatarUrl { get; set; }
    //public string LargeAvatarUrl { get; set; }
    //public bool IsYouFollowed { get; set; }
    //public bool IsHeFollowed { get; set; }

    //public int AskedQuestionsAmount { get; set; }
    //public int AnswersAmount { get; set; }
    //public int FollowersAmount { get; set; }
    //public int FollowedAmount { get; set; }
    //public int FavoritesAmount { get; set; }
    //public int CommentsAmount { get; set; }

    //public UserStatisticsViewModel UserStatistics { get; set; } = new UserStatisticsViewModel();
    //public List<UserContactsViewModel> Social { get; set; } = new List<UserContactsViewModel>();

    public static UserInfoViewModel CreateFromUserInfoDb(UserInfoDb userInfo)
    {
      return new UserInfoViewModel
      {
        BirthDate = userInfo.BirthDate,
        City = userInfo.City,
        Description = userInfo.Description,
        LargetAvatarUrl = MediaConverter.ToFullAvatarUrl(userInfo.OriginaltAvatarUrl, AvatarSizeType.Large),
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(userInfo.OriginaltAvatarUrl, AvatarSizeType.Small),
        Login = userInfo.Login,
        SexType = userInfo.SexType,
        UserId = userInfo.UserId
      };
    }
  }
}