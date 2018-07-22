using System;

namespace QuestionsData.Queries.UserQuestions
{
  public class UserCommentedQuestionDb : QuestionBaseDb
  {
    public int CommentId { get; set; }
    public string CommentText { get; set; }
    public int CommentLikesCount { get; set; }
    public int CommentDislikesCount { get; set; }
    public int YourCommentFeedbackType { get; set; }
    public int? PreviousCommentId { get; set; }
    public DateTime CommentedDate { get; set; }
  }
}