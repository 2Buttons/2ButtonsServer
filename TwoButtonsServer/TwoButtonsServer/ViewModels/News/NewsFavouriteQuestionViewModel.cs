using System;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsFavouriteQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}