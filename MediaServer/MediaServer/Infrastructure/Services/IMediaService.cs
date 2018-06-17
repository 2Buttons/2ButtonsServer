using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.AspNetCore.Http;

namespace MediaServer.Infrastructure.Services
{
  public interface IMediaService
  {
    string GetStandadAvatarUrl(AvatarSizeType sizeType);
    List<string> GetQuestionStandadBackgroundsUrl();
    bool IsAlreadyDownloadedUrl(string url);
    bool IsUrlValid(string url);
    Task<string> UploadAvatar(IFormFile file, AvatarSizeType size);
    string UploadAvatar(string url, AvatarSizeType size);
    Task<string> UploadBackground(IFormFile file);
    string UploadBackground(string url);
  }
}