using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MediaServer.FileSystem
{
    public class FileManager : IFileManager
    {
        private IDictionary<string, string> _folders;

        public IOptions<MediaData> MediaOptions { get; }
        public IHostingEnvironment AppEnvironment { get; }

        public FileManager(IHostingEnvironment appEnvironment, IOptions<MediaData> mediaOptions)
        {
            MediaOptions = mediaOptions;
            AppEnvironment = appEnvironment;
            InitConfiguration();
        }

        public List<string> GetMediaFolders()
        {
            return _folders.Values.ToList();
        }

        public bool IsValidImageType(string imageType)
        {
            return _folders.Keys.Contains(imageType);
        }

        public bool TryConvertHashIntoImageType(string hashCode, out string imageType)
        {
            return _folders.TryGetValue(hashCode, out imageType);
        }

        public string CreateServerPath(string imageType, string imageName)
        {
            return Path.Combine(MediaOptions.Value.RootFolderRelativePath + MediaOptions.Value.RootFolderName,
                imageType, imageName);
        }

        public string GetWebPath(string imageType, string imageName)
        {
            return "/" + imageType + "/" + imageName;
        }

        public string CreateUniqueName(string imageName)
        {
            return Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageName);
        }

        public void InitConfiguration()
        {

            _folders = new Dictionary<string, string>(MediaOptions.Value.MediaFolders.Count);


            foreach (Folder folder in MediaOptions.Value.MediaFolders)
            {
                folder.Name = folder.Name.ToUpper();
                _folders.Add(folder.Name.GetMd5Hash(), folder.Name);
            }
            _folders.Add(MediaOptions.Value.DefaultMediaFolder.GetMd5Hash(), MediaOptions.Value.DefaultMediaFolder);

            CreateIfNotExistsRootFolder();
            foreach (var mediaFolder in _folders.Keys)
                CreateIfNotExistsMediaFolder(mediaFolder);
        }

        private void CreateIfNotExistsRootFolder()
        {
            Directory.CreateDirectory(GetAbsoluteRootPath());
        }

        private void CreateIfNotExistsMediaFolder(string folderName)
        {
            Directory.CreateDirectory(GetAbsoluteMediaFolderPath(folderName));
        }

        private string GetAbsoluteRootPath()
        {
            return Path.Combine(MediaOptions.Value.RootFolderRelativePath + MediaOptions.Value.RootFolderName, "");
        }

        private string GetAbsoluteMediaFolderPath(string folderName)
        {
            return Path.Combine(GetAbsoluteRootPath(), folderName);
        }
    }
}