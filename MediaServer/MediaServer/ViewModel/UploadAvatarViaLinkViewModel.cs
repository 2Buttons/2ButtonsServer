using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadAvatarViaUrlViewModel
  {
    public AvatarType AvatarType { get; set; } = AvatarType.Custom;
    [Required]
    public string Url { get; set; }
  }
}
