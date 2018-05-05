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

    [HttpPost("isUrlValid")]
    public IActionResult IsUrlValid([FromBody]IsUrlValidViewModel url)
    {
      // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу
      if (url == null)
        return BadRequest("Input parameter is null");

      return Ok(new {IsValid = _fileManager.IsUrlValid(url.Url)});
    }

    [HttpPost("uploadUserAvatar")]
    public async Task<IActionResult> UploadUserAvatar(UploadUserAvatarViewModel avatar)
    {
      // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу
      if (avatar == null)
        return BadRequest("Input parameter is null");
      if (avatar.UploadedFile == null)
        return BadRequest("uploadedFile is null");

      var imageType = avatar.Size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(avatar.UploadedFile.FileName);
      var avatarLink = _fileManager.GetWebPath(imageType, uniqueName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      switch (avatar.Size)
      {
        case AvatarSizeType.UserSmallAvatarPhoto:

          using (var fileStream = new FileStream(filePath, FileMode.Create))
          {
            await avatar.UploadedFile.CopyToAsync(fileStream);
          }
          if (UserWrapper.TryUpdateUserSmallAvatar(_context, avatar.UserId, avatarLink))
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
          return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);

        case AvatarSizeType.UserFullAvatarPhoto:
          using (var fileStream = new FileStream(filePath, FileMode.Create))
          {
            await avatar.UploadedFile.CopyToAsync(fileStream);
          }
          if (UserWrapper.TryUpdateUserSmallAvatar(_context, avatar.UserId, avatarLink))
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
          return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);
        default:
          return BadRequest("Image type is not valid.");
      }

    
    }

    [HttpPost("uploadQuestionBackground")]
    public async Task<IActionResult> UploadQuestionBackground(UploadQuestionBackgroundViewModel background)
    {
      if (background == null)
        return BadRequest("Input parameter is null");
      if (background.UploadedFile == null)
        return BadRequest("uploadedFile is null");

      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(background.UploadedFile.FileName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await background.UploadedFile.CopyToAsync(fileStream);
      }

      var backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
      if (QuestionWrapper.TryUpdateQuestionBackgroundLink(_context, background.QuestionId, backgroundLink))
        return Ok(backgroundLink);
      return BadRequest("Link is not saved in the database, but link in file system:" + backgroundLink);
    }
  }
}