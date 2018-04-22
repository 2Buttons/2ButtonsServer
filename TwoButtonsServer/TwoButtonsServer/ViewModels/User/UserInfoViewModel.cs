namespace TwoButtonsServer.ViewModels.User
{
    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public string Description { get; set; }
        public string FullAvatarLink { get; set; }
        public int? YouFollowed { get; set; }
        public int? HeFollowed { get; set; }
        public int? AskedQuestions { get; set; }
        public int? Answers { get; set; }
        public int? Followers { get; set; }
        public int? Followed { get; set; }
        public int? Favorites { get; set; }
        public int? Comments { get; set; }

        public UserStatisticsViewModel UserStatistics { get; set; }
    }

    
}
