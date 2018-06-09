using System;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsRecommendedQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int RecommendedUserId { get; set; }
        public string RecommendedUserLogin { get; set; }
    }
}