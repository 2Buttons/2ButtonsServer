using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadQuestionBackgroundViewModel
  {
    [IdValidationt(nameof(QuestionId))]
    public int QuestionId { get; set; }

    [Required]
    public IFormFile File { get; set; }
  }
}