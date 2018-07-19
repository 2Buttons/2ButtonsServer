using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadBackgroundViaLinkViewModel
  {
    public BackgroundType BackgroundType { get; set; } = BackgroundType.Custom;
    [Required]
    public string Url { get; set; }
  }
}