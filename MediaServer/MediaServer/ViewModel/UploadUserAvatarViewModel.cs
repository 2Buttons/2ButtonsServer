using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadUserAvatarViewModel
  {
    public int UserId { get; set; }
    public AvatarSizeType Size { get; set; }
    public IFormFile UploadedFile { get; set; }
  }
}