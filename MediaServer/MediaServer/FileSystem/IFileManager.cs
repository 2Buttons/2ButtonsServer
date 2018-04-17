using System.Collections.Generic;

namespace MediaServer.FileSystem
{
    public interface IFileManager
    {
        bool IsValidImageType(string imageType);
        string CreateServerPath(string imageType, string imageName);
        string GetWebPath(string imageType, string imageName);
        string CreateUniqueName(string imageName);
        bool TryConvertHashIntoImageType(string hashCode, out string imageType);
        List<string> GetMediaFolders();
    }
}