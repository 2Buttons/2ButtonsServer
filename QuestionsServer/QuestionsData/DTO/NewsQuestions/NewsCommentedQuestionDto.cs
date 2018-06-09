using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
  public class NewsCommentedQuestionDto : NewsQuestionBaseDto
  {
    public NewsCommentedQuestionDb NewsCommentedQuestionDb { get; set; }
  }
}