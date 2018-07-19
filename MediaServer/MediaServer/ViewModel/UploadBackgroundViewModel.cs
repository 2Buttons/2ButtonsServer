using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadBackgroundViewModel
  {
    public BackgroundType BackgroundType { get; set; } = BackgroundType.Custom;
    [Required]
    public IFormFile File { get; set; }
  }
}