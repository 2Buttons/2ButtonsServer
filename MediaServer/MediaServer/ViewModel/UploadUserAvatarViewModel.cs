using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadUserAvatarViewModel
  {
    public AvatarSizeType Size { get; set; }
    [Required]
    public IFormFile File { get; set; }
  }
}