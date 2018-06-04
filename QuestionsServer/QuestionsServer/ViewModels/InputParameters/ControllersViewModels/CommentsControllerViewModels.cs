using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddCommentViewModel : QuestionIdViewModel
  {
    [Required]
    public string CommentText { get; set; }

    public int? PreviousCommentId { get; set; } = null;
  }

  public class AddCommentFeedbackViewModel : UserIdViewModel
  {
    [Required]
    [IdValidation(nameof(CommentId))]
    public int CommentId { get; set; }

    public QuestionFeedbackType FeedbackType { get; set; }
  }

  public class GetCommentsViewModel : QuestionIdViewModel
  {
    public int Amount { get; set; } = 10000;
  }
}