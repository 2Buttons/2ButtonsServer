using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace MediaServer.ViewModel
{
  public class CopyAvatars
  {
    [Required]
    public string SourceFolder { get; set; }

    [Required]
    public AvatarSizeType CopyToAvatarSizeType { get; set; }

    [Required]
    public AvatarType CopyToNewAvatarType { get; set; }
  }
}