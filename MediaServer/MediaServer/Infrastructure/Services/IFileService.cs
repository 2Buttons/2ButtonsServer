using System.Collections.Generic;

namespace MediaServer.Infrastructure.Services
{
  public interface IFileService
  {
    bool IsUrlValid(string url);
    bool IsValidImageType(string imageType);
    string CreateServerPath(string imageType, string imageName);
    string GetWebPath(string imageType, string imageName);
    string CreateUniqueName(string imageName);
    bool TryConvertHashIntoImageType(string hashCode, out string imageType);
    List<string> GetMediaFolders();
  }
}