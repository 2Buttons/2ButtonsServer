using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsRecommendedQuestionDb : NewsQuestionBaseDb
  {
    public int RecommendedUserId { get; set; }
    public string RecommendedUserLogin { get; set; }
    public SexType RecommendedUserSexType { get; set; }
  }
}