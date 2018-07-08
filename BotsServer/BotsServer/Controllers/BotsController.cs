using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BotsData;
using BotsServer.ViewModels.Input;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;

namespace BotsServer.Controllers
{
  [Route("TwoButtons1234567890TwoButtons/bots")]
  [ApiController]
  public class BotsController : ControllerBase
  {
    private readonly BotsUnitOfWork _uow;
    public BotsController(BotsUnitOfWork uow)
    {
      _uow = uow;
    }

    [HttpPost("questions")]
    public async Task<IActionResult> GetQuestions([FromBody]GetQuestions vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      var result = await _uow.QuestionRepository.GetQuestions(vm.PageParams.Offset, vm.PageParams.Count);
      return new OkResponseResult(result);
    }

    [HttpPost("questions/pattern")]
    public async Task<IActionResult> GetQuestionsByPattern([FromBody]GetGuestionsByPatternViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      var result = await _uow.QuestionRepository.GetQuestionsByPattern(vm.Pattern, vm.PageParams.Offset, vm.PageParams.Count);
      return new OkResponseResult(result);
    }

    [HttpPost("questions/id")]
    public async Task<IActionResult> GetQuestionsById([FromBody]GetQuestionByIdViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      var result = await _uow.QuestionRepository.GetQuestionById(vm.QuestionId);
      return new OkResponseResult(result);
    }


    [HttpPost]
    public async Task<IActionResult> GetBotsCount([FromBody]GetBotsCount vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      var result = _uow.BotsRepository.GetBotsCount();
      return new OkResponseResult(result);
    }

    [HttpPost("magic")]
    public IActionResult Magic([FromBody]MagicViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      
    }


  }
}
