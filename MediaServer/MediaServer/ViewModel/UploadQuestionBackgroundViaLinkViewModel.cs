using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadQuestionBackgroundViaLinkViewModel
  {
    [IdValidationt(nameof(QuestionId))]
    public int QuestionId { get; set; }

    [Required]
    public string Url { get; set; }
  }
}