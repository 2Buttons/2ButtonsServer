using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsViewModel
    {
        public List<NewsAskedQuestionViewModel> NewsAskedQuestions { get; set; }
        public List<NewsAnsweredQuestionViewModel> NewsAnsweredQuestions { get; set; }
        public List<NewsFavouriteQuestionViewModel> NewsFavouriteQuestions { get; set; }
        public List<NewsRecommendedQuestionViewModel> NewsRecommendedQuestions { get; set; }
        public List<NewsCommentedQuestionViewModel> NewsCommentedQuestions { get; set; }
    }
}
