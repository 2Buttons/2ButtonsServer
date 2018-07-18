using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;
using QuestionsData.Queries;

namespace QuestionsData.DTO
{
  public class UserCommentedQuestionDto : QuestionBaseDb
  {
    public List<UserCommentQuestionDto> Comments { get; set; }
  }

  public class UserCommentQuestionDto
  {
    public int CommentId { get; set; }
    public string Text { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
    public int? PreviousCommentId { get; set; }
    public DateTime AddDate { get; set; }
  }
}
