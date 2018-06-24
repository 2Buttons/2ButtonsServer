using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace MediaServer.Infrastructure.Services
{
  public interface IFileService
  {
    IHostingEnvironment AppEnvironment { get; }
    MediaSettings MediaOptions { get; }

    string CreateServerPath(string imageType, string imageName);
    string CreateUniqueName(string imageName);
    string GetAbsoluteMediaFolderPath(string folderName);
    string GetAbsoluteMediaRootPath();
    List<string> GetMediaFolders();
    string GetWebPath(string imageType, string imageName);
    void InitConfiguration();
    bool IsUrlValid(string url);
    bool IsValidImageType(string imageType);
    bool TryConvertHashIntoImageType(string hashCode, out string imageType);
  }
}