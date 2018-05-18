using System;

namespace TwoButtonsDatabase.Entities.UserQuestions
{
    public partial class RecommendedQuestionDb : QuestionBaseDb
    {
      public int ToUserId { get; set; }
      public string ToUserLogin { get; set; }
      public DateTime RecommendDate { get; set; }
  }
}
