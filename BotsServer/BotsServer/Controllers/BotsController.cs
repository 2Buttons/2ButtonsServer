using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsServer.ViewModels.Input;
using Microsoft.AspNetCore.Mvc;

namespace BotsServer.Controllers
{
    [Route("TwoButtons123456790TwoButtons/bots")]
    [ApiController]
    public class BotsController : ControllerBase
    {
        
        [HttpGet("questions")]
        public ActionResult GetQuestions([FromBody]PageParams vm)
        {
            return new string[] { "value1", "value2" };
        }

      [HttpGet("questions/pattern")]
      public ActionResult GetQuestionsByPattern([FromBody]GetGuestionsByPatternViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }

      [HttpGet("questions/id")]
      public ActionResult GetQuestionsById([FromBody]GetQuestionByIdViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }

      [HttpGet("magic")]
      public ActionResult GetQuestionsByPattern([FromBody]GetQuestionByIdViewModel vm)
      {
        return new string[] { "value1", "value2" };
      }


  }
}
