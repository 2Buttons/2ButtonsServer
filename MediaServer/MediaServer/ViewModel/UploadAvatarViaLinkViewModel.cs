using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadAvatarViaLinkViewModel
  {
    [Required]
    public AvatarSizeType Size { get; set; }
    [Required]
    public string Url { get; set; }
  }
}
