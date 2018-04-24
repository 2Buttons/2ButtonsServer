using System;

namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsFavouriteQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}