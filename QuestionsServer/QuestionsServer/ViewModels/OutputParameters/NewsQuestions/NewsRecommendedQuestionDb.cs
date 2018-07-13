using System.Collections.Generic;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
  public class NewsRecommendedQuestionViewModel : NewsQuestionBaseViewModel
  {
    public List<NewsUserViewModel> RecommendedUsers { get; set; }
  }
}