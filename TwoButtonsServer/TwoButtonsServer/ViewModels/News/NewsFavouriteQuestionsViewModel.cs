using System;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsFavouriteQuestionsViewModel : QuestionBaseViewModel
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}