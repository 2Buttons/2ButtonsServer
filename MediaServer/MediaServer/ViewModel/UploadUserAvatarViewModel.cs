using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
    public class UploadUserAvatarViewModel
    {
        public int UserId { get; set; }
        public string Size { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
