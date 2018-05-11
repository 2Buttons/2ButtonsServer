using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
    public class UploadQuestionBackgroundViaLinkViewModel
    {
      public int QuestionId { get; set; }
      public string Url { get; set; }
  }
}
