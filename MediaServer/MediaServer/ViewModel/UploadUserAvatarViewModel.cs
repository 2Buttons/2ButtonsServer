using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadUserAvatarViewModel
  {
    [IdValidationt(nameof(UserId))]
    public int UserId { get; set; }
    public AvatarSizeType Size { get; set; }
    [Required]
    public IFormFile File { get; set; }
  }
}