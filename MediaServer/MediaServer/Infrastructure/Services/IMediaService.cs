using System.Threading.Tasks;
using CommonLibraries;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaServer.Infrastructure.Services
{
  public interface IMediaService
  {
    bool IsUrlValid(string url);
    string GetStandadAvatarUrl(AvatarSizeType size);
    string GetStandadQuestionBackgroundUrl();
    Task<string> UploadAvatar(IFormFile file, AvatarSizeType size);
    string UploadAvatar(string url, AvatarSizeType size);
    Task<string> UploadBackground(IFormFile file);
    string UploadBackground(string url);
  }
}