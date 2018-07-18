using System;
using System.Collections.Generic;
using CommonLibraries;
using QuestionsData.DTO;

namespace QuestionsServer.ViewModels.OutputParameters.UserQuestions
{
    public class UserCommentedQuestionsViewModel : QuestionBaseViewModel
    {
      public List<UserCommentQuestionDto> Comments { get; set; } = new List<UserCommentQuestionDto>();
    }
}

public class UserCommentQuestionViewModel
{
  public int CommentId { get; set; }
  public string Text { get; set; }
  public int LikesAmount { get; set; }
  public int DislikesAmount { get; set; }
  public FeedbackType YourFeedbackType { get; set; }
  public int PreviousCommentId { get; set; }
  public DateTime AddDate { get; set; }
}