using System.Collections.Generic;
using CommonLibraries;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserInfoViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public int Age { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string LargeAvatarUrl { get; set; }
    public string SmallAvatarUrl { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }

    public int AskedQuestionsCount { get; set; }
    public int AnswersCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowedCount { get; set; }
    public int FavoritesCount { get; set; }
    public int CommentsCount { get; set; }

    public UserStatisticsViewModel UserStatistics { get; set; } = new UserStatisticsViewModel();
    public List<UserContactsViewModel> Social { get; set; } = new List<UserContactsViewModel>();
  }
}