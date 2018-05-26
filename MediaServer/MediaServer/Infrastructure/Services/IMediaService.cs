using System.Threading.Tasks;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaServer.Infrastructure.Services
{
  public interface IMediaService
  {
    bool IsUrlValid(string url);
    Task<string> UploadAvatar(int userId, IFormFile file, AvatarSizeType size);
    Task<string> UploadAvatar(int userId, string url, AvatarSizeType size);
    Task<string> UploadBackground(int questionId, IFormFile file);
    Task<string> UploadBackground(int questionId, string url);
  }
}