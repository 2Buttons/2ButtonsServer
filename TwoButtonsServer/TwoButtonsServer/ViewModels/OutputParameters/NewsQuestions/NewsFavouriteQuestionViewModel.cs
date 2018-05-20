using System;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsFavoriteQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}