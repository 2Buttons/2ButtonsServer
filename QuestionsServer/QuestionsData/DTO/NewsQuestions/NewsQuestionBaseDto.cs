using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
  public class NewsQuestionBaseDto : NewsQuestionBaseDb
  {
    public int Priority { get; set; }
  }
}