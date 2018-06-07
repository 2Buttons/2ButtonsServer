using System;

namespace QuestionsData.Queries.UserQuestions
{
    public partial class RecommendedQuestionDb : QuestionBaseDb
    {
      public int ToUserId { get; set; }
      public string ToUserLogin { get; set; }
      public DateTime RecommendDate { get; set; }
  }
}
