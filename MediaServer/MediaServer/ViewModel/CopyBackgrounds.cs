using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace MediaServer.ViewModel
{
  public class CopyBackgrounds
  {
    [Required]
    public string SourceFolder { get; set; }

    [Required]
    public BackgroundSizeType CopyToBackgroundSizeType { get; set; }

    [Required]
    public BackgroundType CopyToNewBackgroundType { get; set; }
  }
}