using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
    public class UploadQuestionBackgroundViewModel
    {
        public int QuestionId { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
