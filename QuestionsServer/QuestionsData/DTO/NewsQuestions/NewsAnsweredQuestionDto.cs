using Newtonsoft.Json;
using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
    public class NewsAnsweredQuestionDto : NewsQuestionBaseDto
    {
      public NewsAnsweredQuestionDb NewsAnsweredQuestionDb { get; set; }
  }
}