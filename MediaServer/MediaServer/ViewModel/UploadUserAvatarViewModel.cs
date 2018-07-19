using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadUserAvatarViewModel
  {
    public AvatarType AvatarType { get; set; } = AvatarType.Custom;
    [Required]
    public IFormFile File { get; set; }
  }
}