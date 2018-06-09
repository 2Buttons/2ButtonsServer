using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
  public class NewsAskedQuestionDto : NewsQuestionBaseDto
  {
    public NewsAskedQuestionDb NewsAskedQuestionDb { get; set; }
  }
}