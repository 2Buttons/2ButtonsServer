using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
  public class NewsFavoriteQuestionDto : NewsQuestionBaseDto
  {
    public NewsFavoriteQuestionDb NewsFavoriteQuestionDb { get; set; }
  }
}