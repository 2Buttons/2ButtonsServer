using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace MediaServer.ViewModel
{
  public class UploadAvatarViaLinkViewModel
  {
    [IdValidationt(nameof(UserId))]
    public int UserId { get; set; }
    public AvatarSizeType Size { get; set; }
    [Required]
    public string Url { get; set; }
  }
}
