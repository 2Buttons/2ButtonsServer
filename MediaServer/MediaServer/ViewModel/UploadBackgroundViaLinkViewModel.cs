using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadBackgroundViaUrlViewModel
  {
    public BackgroundType BackgroundType { get; set; } = BackgroundType.Custom;
    [Required]
    public string Url { get; set; }
  }
}