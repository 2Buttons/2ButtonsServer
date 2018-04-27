using System.Collections.Generic;

namespace TwoButtonsServer.ViewModels.OutputParameters.User
{
    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public string Description { get; set; }
        public string FullAvatarLink { get; set; }
        public bool IsYouFollowed { get; set; }
        public bool IsHeFollowed { get; set; }
        public int AskedQuestionsAmount { get; set; }
        public int AnswersAmount { get; set; }
        public int FollowersAmount { get; set; }
        public int FollowedAmount { get; set; }
        public int FavoritesAmount { get; set; }
        public int CommentsAmount { get; set; }

        public UserStatisticsViewModel UserStatistics { get; set; } = new UserStatisticsViewModel();
        public List<UserContactsViewModel> Social { get; set; } = new List<UserContactsViewModel>();
    }

    
}
