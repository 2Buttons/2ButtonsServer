using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadQuestionBackgroundViewModel
  {
    [Required]
    public IFormFile File { get; set; }
  }
}