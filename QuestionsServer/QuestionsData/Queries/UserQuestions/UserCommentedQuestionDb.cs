using System;

namespace QuestionsData.Queries.UserQuestions
{
  public class UserCommentedQuestionDb : QuestionBaseDb
  {
    public int CommentId { get; set; }
    public string CommentText { get; set; }
    public int CommentLikes { get; set; }
    public int CommentDislikes { get; set; }
    public int YourCommentFeedback { get; set; }
    public int? PreviousCommentId { get; set; }
    public DateTime CommentAddDate { get; set; }
  }
}