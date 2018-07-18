using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Queries.UserQuestions
{
  public partial class UserCommentedQuestionDb : QuestionBaseDb
  {
    public int CommentId { get; set; }
    [Column("comment")]
    public string Text { get; set; }
    public int CommentLikes { get; set; }
    public int CommentDislikes { get; set; }
    public int YourCommentFeedback { get; set; }
    public int? PreviousCommentId { get; set; }
    public DateTime CommentAddDate { get; set; }
  }
}