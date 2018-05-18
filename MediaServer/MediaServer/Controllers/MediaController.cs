using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MediaServer.FileSystem;
using MediaServer.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;

namespace MediaServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("images")]
  public class MediaController : Controller
  {
    private readonly TwoButtonsUnitOfWork _mainDb;
    private readonly IFileManager _fileManager;

    public MediaController(TwoButtonsUnitOfWork mainDb, IHostingEnvironment appEnvironment, IFileManager fileManager)
    {
      _mainDb = mainDb;
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
    public IActionResult IsUrlValid([FromBody] IsUrlValidViewModel url)
    {
      // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу
      if (url == null)
        return BadRequest("Input parameter is null");

      return Ok(new {IsValid = _fileManager.IsUrlValid(url.Url)});
    }

    [HttpPost("uploadUserAvatarViaLink")]
    public IActionResult UploadUserAvatarViaLink([FromBody] UploadAvatarViaLinkViewModel avatar)
    {
      // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу
      if (avatar == null)
        return BadRequest("Input parameter is null");
      if (string.IsNullOrEmpty(avatar.Url))
        return BadRequest("Url is null");

      var imageType = avatar.Size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(avatar.Url);
      var avatarLink = _fileManager.GetWebPath(imageType, uniqueName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      new WebClient().DownloadFileAsync(new Uri(avatar.Url), filePath);

      switch (avatar.Size)
      {
        case AvatarSizeType.UserSmallAvatarPhoto:

          if (_mainDb.Accounts.TryUpdateUserSmallAvatar(avatar.UserId, avatarLink))
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
          return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);

        case AvatarSizeType.UserFullAvatarPhoto:
          if (_mainDb.Accounts.TryUpdateUserFullAvatar(avatar.UserId, avatarLink))
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
          return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);
        default:
          return BadRequest("Image type is not valid.");
      }
    }

    [HttpPost("uploadQuestionBackgroundViaLink")]
    public  IActionResult UploadQuestionBackgroundViaLink([FromBody]UploadQuestionBackgroundViaLinkViewModel background)
    {
      if (background == null)
        return BadRequest("Input parameter is null");
      if (string.IsNullOrEmpty(background.Url))
        return BadRequest("Url is null");

      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(background.Url);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      new WebClient().DownloadFileAsync(new Uri(background.Url), filePath);

      var backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
      if (_mainDb.Questions.TryUpdateQuestionBackgroundLink(background.QuestionId, backgroundLink))
        return Ok(backgroundLink);
      return BadRequest("Link is not saved in the database, but link in file system:" + backgroundLink);
    }

    [HttpPost("uploadUserAvatar")]
    public async Task<IActionResult> UploadUserAvatar(UploadUserAvatarViewModel avatar)
    {
      // проверить есть ли такой пользователь если нет то возвратить ошибку, если есть, то отправить запрос к бд, чтобы добавла ссылку на фото и возвратить ссылку на страницу
      if (avatar == null)
        return BadRequest("Input parameter is null");
      if (avatar.File == null)
        return BadRequest("uploadedFile is null");

      var imageType = avatar.Size.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(avatar.File.FileName);
      var avatarLink = _fileManager.GetWebPath(imageType, uniqueName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await avatar.File.CopyToAsync(fileStream);
      }
      switch (avatar.Size)
      {
        case AvatarSizeType.UserSmallAvatarPhoto:


          if (_mainDb.Accounts.TryUpdateUserSmallAvatar(avatar.UserId, avatarLink))
            return Ok(_fileManager.GetWebPath(imageType, uniqueName));
          return BadRequest("Link is not saved in the database, but link in file system:" + avatarLink);

        case AvatarSizeType.UserFullAvatarPhoto:
          if (_mainDb.Accounts.TryUpdateUserFullAvatar(avatar.UserId, avatarLink))
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
      if (background.File == null)
        return BadRequest("uploadedFile is null");

      var imageType = BackgroundType.Background.ToString().GetMd5Hash();
      var uniqueName = _fileManager.CreateUniqueName(background.File.FileName);
      var filePath = _fileManager.CreateServerPath(imageType, uniqueName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await background.File.CopyToAsync(fileStream);
      }

      var backgroundLink = _fileManager.GetWebPath(imageType, uniqueName);
      if (_mainDb.Questions.TryUpdateQuestionBackgroundLink(background.QuestionId, backgroundLink))
        return Ok(backgroundLink);
      return BadRequest("Link is not saved in the database, but link in file system:" + backgroundLink);
    }
  }
}