using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaServer.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace MediaServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    //[Route("api/[controller]")]
    public class MediaController : Controller
    {

        TwoButtonsContext _context;
        IHostingEnvironment _appEnvironment;

        public MediaController(TwoButtonsContext context, IHostingEnvironment appEnvironment, IFileManager fileManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }


        // GET api/values
        [HttpGet("images")]
        public FileResult DownloadFile(string path)
        {
            
            
            string fileName = Path.GetFileName(path);
            var ext = Path.GetExtension(fileName);
            string fileType = "application/" + ext.Skip(1).FirstOrDefault();

            string absolutePath = Path.Combine(_appEnvironment.ContentRootPath, "..\\..\\..\\MediaData\\"+fileName);


            FileStream fs = new FileStream(absolutePath, FileMode.Open);
            
            return  File(fs, fileType, fileName);
        }



        // POST api/values
        [HttpPost("images")]
        public async Task<IActionResult> UploadFile(int userId, string imageType, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                //путь к папке Files
                string path = "\\..\\..\\..\\..\\MediaData\\" + uploadedFile.FileName;

                var absolutePath = _appEnvironment.WebRootPath + path;
                // сохраняем фалй в папкуFiles в каталоге 
                using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return Ok("All is good");
            }

            return BadRequest("The file is not uploaded");
        }

    

    }
}
