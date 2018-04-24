using System.Collections.Generic;

namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsViewModel
    {
        public List<NewsAskedQuestionViewModel> NewsAskedQuestions { get; set; }
        public List<NewsAnsweredQuestionViewModel> NewsAnsweredQuestions { get; set; }
        public List<NewsFavoriteQuestionViewModel> NewsFavoriteQuestions { get; set; }
        public List<NewsRecommendedQuestionViewModel> NewsRecommendedQuestions { get; set; }
        public List<NewsCommentedQuestionViewModel> NewsCommentedQuestions { get; set; }
    }
}
