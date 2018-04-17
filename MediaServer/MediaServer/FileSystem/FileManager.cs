using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MediaServer.FileSystem
{
    public class FileManager : IFileManager
    {
        private string _defaultMediaFolder;
        private IDictionary<string, string> _folders;

        public string RootFolderName { get; private set; }
        public string RootFolderRelativePath { get; private set; }
        public IConfiguration AppConfiguration { get; }
        public IHostingEnvironment AppEnvironment { get; }

        public FileManager(IHostingEnvironment appEnvironment, IConfiguration configuration)
        {
            AppConfiguration = configuration;
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
            return Path.Combine(RootFolderRelativePath + RootFolderName,
                imageType, imageName);
        }

        public string GetWebPath(string imageType, string imageName)
        {
            return "/" + imageType.GetMd5Hash() + "/" + imageName;
        }

        public string CreateUniqueName(string imageName)
        {
            return Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageName);
        }

        public void InitConfiguration()
        {
            var mediaData = AppConfiguration.GetSection("MediaData");
            RootFolderName = mediaData["RootFolderName"].ToUpper();
            RootFolderRelativePath = mediaData["RootFolderRelativePath"].ToUpper();
            _defaultMediaFolder = mediaData["DefaultMediaFolder"].ToUpper();
            var mediaFoldersCount = mediaData.GetValue<int>("MediaFoldersCount");
            _folders = new Dictionary<string, string>(mediaFoldersCount);


            for (var i = 0; i < mediaFoldersCount; i++)
            {
                var folderName = mediaData.GetValue<string>($"MediaFolders:{i}:Name");
                folderName = folderName.ToUpper();
                _folders.Add(folderName.GetMd5Hash(), folderName);
            }
            _folders.Add(_defaultMediaFolder.GetMd5Hash(), _defaultMediaFolder);

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
            return Path.Combine(RootFolderRelativePath + RootFolderName,"");
        }

        private string GetAbsoluteMediaFolderPath(string folderName)
        {
            return Path.Combine(GetAbsoluteRootPath(), folderName);
        }
    }
}