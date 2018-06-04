using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace AccountServer.ViewModels.InputParameters
{
  public class FeedbackViewModel
  {
    [Required]
    [IdValidation]
    public int UserId { get; set; }

    [Required]
    public FeedbackType Type { get; set; }

    [Required]
    public string Text { get; set; }
  }
}