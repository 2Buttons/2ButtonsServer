using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MediaServer.FileSystem
{


    public class FileManager : IFileManager
    {
        private IDictionary<int, string> _folders;
        private string _defaultMediaFolder;

        public string RootFolder { get; private set; }

        public IConfiguration AppConfiguration { get; }
        public FileManager(IConfiguration configuration)
        {
            AppConfiguration = configuration;
            InitConfiguration();
        }

        public void InitConfiguration()
        {
            var mediaData = AppConfiguration.GetSection("MediaData");
            RootFolder = mediaData["RootFolder"];
            _defaultMediaFolder = mediaData["DefaultMediaFolder"];
            var folderNames = mediaData.GetSection("MediaFolders").GetChildren().Select(x=>x.Value);
            _folders = new Dictionary<int, string>(folderNames.Count());
            foreach (var folderName in folderNames)
            {
                _folders.Add(folderName.GetHashCode(), folderName);
            }            
        }
    }
}
