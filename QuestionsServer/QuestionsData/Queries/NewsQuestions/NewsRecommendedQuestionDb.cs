using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsRecommendedQuestionDb : NewsQuestionBaseDb
  {
    public int RecommendedUserId { get; set; }
    public string RecommendedUserFirstName { get; set; }
    public string RecommendedUserLastName { get; set; }
    public SexType RecommendedUserSexType { get; set; }
  }
}