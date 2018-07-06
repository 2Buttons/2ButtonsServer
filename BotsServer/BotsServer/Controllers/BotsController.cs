using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BotsServer.ViewModels.Input;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;

namespace BotsServer.Controllers
{
    [Route("TwoButtons123456790TwoButtons/bots")]
    [ApiController]
    public class BotsController : ControllerBase
    {
        
        [HttpGet("questions")]
        public IActionResult GetQuestions([FromBody]GetQuestions vm)
        {
          if (vm.Code != "MySecretCode!123974_QQ")
            return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
            
        }

      [HttpGet("questions/pattern")]
      public IActionResult GetQuestionsByPattern([FromBody]GetGuestionsByPatternViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }

      [HttpGet("questions/id")]
      public IActionResult GetQuestionsById([FromBody]GetQuestionByIdViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }

      [HttpGet("magic")]
      public IActionResult GetQuestionsByPattern([FromBody]MagicViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }


  }
}
