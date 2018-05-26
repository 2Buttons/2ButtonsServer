using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddCommentViewModel : QuestionIdViewModel
  {
    [Required]
    public string CommentText { get; set; }

    public int PreviousCommnetId { get; set; } = 0;
  }

  public class AddCommentFeedbackViewModel : UserIdViewModel
  {
    [Required]
    public int CommentId { get; set; }

    public FeedbackType FeedbackType { get; set; }
  }

  public class GetCommentsViewModel : QuestionIdViewModel
  {
    public int Amount { get; set; } = 10000;
  }
}