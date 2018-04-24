using System;

namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsRecommendedQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int RecommendedUserId { get; set; }
        public string RecommendedUserLogin { get; set; }
        public DateTime RecommendedDate { get; set; }
    }
}