using System;
using System.IO;
using System.Threading.Tasks;
using MediaServer.FileSystem;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;

namespace MediaServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Route("images")]
    public class MediaController : Controller
    {
        private TwoButtonsContext _context;
        private readonly IFileManager _fileManager;

        public MediaController(TwoButtonsContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }


        // GET api/values
        [HttpGet("downloadFile/{imageType}/{fileName}")]
        public IActionResult DownloadFile(int userId, string imageType, string fileName)
        {

            if (string.IsNullOrEmpty(imageType))
                return BadRequest($"{imageType} is empty or null");
            if (string.IsNullOrEmpty(fileName))
                return BadRequest($"{fileName} is empty or null");
            if (fileName == "favicon.icon")
                return Ok();

            if (!_fileManager.IsValidImageType(imageType))
                return BadRequest("ImageType is uncleared.");

            var fileExtension = Path.GetExtension(fileName).Replace(".", "");
            var fileType = "application/" + fileExtension;

            var filePath = _fileManager.CreateServerPath(imageType, fileName);
            if (!new FileInfo(filePath).Exists)
                return BadRequest("Oooops.... We can't find file. The Black Hole maybe contains your file.");

            var fs = new FileStream(filePath, FileMode.Open);

            return File(fs, fileType, fileName);
        }

        [HttpGet("getMediaFolders")]
        public IActionResult GetMediaFolders()
        {
            return Ok(_fileManager.GetMediaFolders());
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile(int userId, string imageType, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return BadRequest("The file is not uploaded");
            imageType = imageType.ToUpper().GetMd5Hash();
            if (!_fileManager.IsValidImageType(imageType))
                return BadRequest("Image type is not right.");

            var uniqueName = _fileManager.CreateUniqueName(uploadedFile.FileName);
            var filePath = _fileManager.CreateServerPath(imageType, uniqueName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
        }
    }
}