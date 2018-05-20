using System.Collections.Generic;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsViewModel
    {
        public List<NewsAskedQuestionViewModel> NewsAskedQuestions { get; set; } =
            new List<NewsAskedQuestionViewModel>();

        public List<NewsAnsweredQuestionViewModel> NewsAnsweredQuestions { get; set; } =
            new List<NewsAnsweredQuestionViewModel>();

        public List<NewsFavoriteQuestionViewModel> NewsFavoriteQuestions { get; set; } =
            new List<NewsFavoriteQuestionViewModel>();

        public List<NewsRecommendedQuestionViewModel> NewsRecommendedQuestions { get; set; } =
            new List<NewsRecommendedQuestionViewModel>();

        public List<NewsCommentedQuestionViewModel> NewsCommentedQuestions { get; set; } =
            new List<NewsCommentedQuestionViewModel>();
    }
}
