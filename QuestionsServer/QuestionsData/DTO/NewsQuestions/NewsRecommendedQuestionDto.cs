using System.Collections.Generic;
using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
  public class NewsRecommendedQuestionDto : NewsQuestionBaseDto
  {
    public List<RecommendedUserDto> RecommendedUsers { get; set; }
  }
}