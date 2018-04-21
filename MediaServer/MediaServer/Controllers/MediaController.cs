using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediaServer.FileSystem;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace MediaServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("images")]
    public class MediaController : Controller
    {
        private readonly TwoButtonsContext _context;
        private readonly IFileManager _fileManager;

        public MediaController(TwoButtonsContext context, IHostingEnvironment appEnvironment, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }


        
        //[HttpGet("{imageType:required}/{fileName:required}")]
        //public IActionResult DownloadFile(int userId, string imageType, string fileName)
        //{

        //    if (string.IsNullOrEmpty(imageType))
        //        return BadRequest($"{imageType} is empty or null");
        //    if (string.IsNullOrEmpty(fileName))
        //        return BadRequest($"{fileName} is empty or null");
        //    if (fileName == "favicon.ico")
        //        return Ok();

        //    if (!_fileManager.IsValidImageType(imageType))
        //        return BadRequest("ImageType is uncleared.");

        //    var fileExtension = Path.GetExtension(fileName).Replace(".", "");
        //    var fileType = "image/" + fileExtension;

        //    var filePath = _fileManager.CreateServerPath(imageType, fileName);
        //    if (!new FileInfo(filePath).Exists)
        //        return BadRequest("Oooops.... We can't find file. The Black Hole maybe contains your file.");

        //    var fs = new FileStream(filePath, FileMode.Open);

        //    return File(fs, fileType, fileName);
        //}

        // GET api/values
        //[HttpGet("{imageType:required}/{fileName:required}")]
        //public IActionResult GetStaticFile(int userId, string imageType, string fileName)
        //{

        //    //if (string.IsNullOrEmpty(imageType))
        //    //    return BadRequest($"{imageType} is empty or null");
        //    //if (string.IsNullOrEmpty(fileName))
        //    //    return BadRequest($"{fileName} is empty or null");
        //    //if (fileName == "favicon.ico")
        //    //    return Ok();

        //    //if (!_fileManager.IsValidImageType(imageType))
        //    //    return BadRequest("ImageType is uncleared.");

        //    var fileExtension = Path.GetExtension(fileName).Replace(".", "");
        //    var fileType = "image/" + fileExtension;

        //    var filePath =Directory.GetCurrentDirectory()+ "\\"+_fileManager.CreateServerPath(imageType, fileName);
        //    //if (!new FileInfo(filePath).Exists)
        //    //    return BadRequest("Oooops.... We can't find file. The Black Hole maybe contains your file.");

        //    //var fs = new FileStream(filePath, FileMode.Open);

        //    return (@"E:\Projects\2Buttons\Project\MEDIADATA\87ed58ce5596142e11cb65deb049bb4b\b792bfb3a4ba43108a0033b521a824aa.jpg");
        //}

        [HttpGet("getMediaFolders")]
        public IActionResult GetMediaFolders()
        {
            return Ok("Hello");
        }

        [HttpPost("uploadUserAvatar")]
        public async Task<IActionResult> UploadFile(int userId, string size, IFormFile uploadedFile)
        {
            // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу

            if (uploadedFile == null)
                return BadRequest("uploadedFile is null");
            if (size == null)
                return BadRequest("size is null");

            var imageType = size.ToUpper() == "small".ToUpper() ? "UserSmallAvatarPhoto" : "UserFullAvatarPhoto";

            imageType = imageType.ToUpper().GetMd5Hash();
            if (!_fileManager.IsValidImageType(imageType))
                return BadRequest("Image type is not valid.");

            var uniqueName = _fileManager.CreateUniqueName(uploadedFile.FileName);
            var filePath = _fileManager.CreateServerPath(imageType, uniqueName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            string avatarLink = _fileManager.GetWebPath(imageType, uniqueName);

            if (size.ToUpper() == "small".ToUpper())
            {
                if(UserWrapper.TryUpdateUserSmallAvatar(_context,userId, avatarLink))
                    return Ok(_fileManager.GetWebPath(imageType, uniqueName));
                return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);
            }
            else
            {
                if (UserWrapper.TryUpdateUserFullAvatar(_context, userId, avatarLink))
                    return Ok(_fileManager.GetWebPath(imageType, uniqueName));
                return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);

            }
        }

        [HttpPost("uploadQuestionBackground")]
        public async Task<IActionResult> UploadQuestionBackground(int questionId, IFormFile uploadedFile)
        {
            // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу

            if (uploadedFile == null)
                return BadRequest("uploadedFile is null");
            
            string imageType = "Background".ToUpper().GetMd5Hash();
            if (!_fileManager.IsValidImageType(imageType))
                return BadRequest("Image type is not valid.");

            var uniqueName = _fileManager.CreateUniqueName(uploadedFile.FileName);
            var filePath = _fileManager.CreateServerPath(imageType, uniqueName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            string backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
            if (QuestionWrapper.TryUpdateQuestionBackground(_context, questionId, backgroundLink))
                return Ok(backgroundLink);
            return BadRequest("Link is not saved in the database, but link in file system:"+ backgroundLink);
        }
    }
}