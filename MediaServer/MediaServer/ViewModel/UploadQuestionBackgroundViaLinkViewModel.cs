using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadQuestionBackgroundViaLinkViewModel
  {
    [Required]
    public string Url { get; set; }
  }
}