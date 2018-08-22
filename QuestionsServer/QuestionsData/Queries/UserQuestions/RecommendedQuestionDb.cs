using System;

namespace QuestionsData.Queries.UserQuestions
{
  public class RecommendedQuestionDb : QuestionBaseDb
  {
    public int ToUserId { get; set; }
    public string ToUserFirstName { get; set; }
    public string ToUserLastName { get; set; }
    public DateTime RecommendDate { get; set; }
  }
}