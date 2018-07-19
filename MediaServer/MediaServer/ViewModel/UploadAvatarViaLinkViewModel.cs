using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadAvatarViaLinkViewModel
  {
    public AvatarType AvatarType { get; set; } = AvatarType.Custom;
    [Required]
    public string Url { get; set; }
  }
}
