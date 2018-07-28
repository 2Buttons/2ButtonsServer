using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BotsData;
using BotsData.Contexts;
using BotsServer.Jobs;
using BotsServer.ViewModels.Input;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BotsServer.Controllers
{
  [Route("TwoButtons1234567890TwoButtons/bots")]
  [ApiController]
  public class BotsController : ControllerBase
  {
    private readonly BotsUnitOfWork _uow;
    private readonly BotsManager _botsManager;
    private readonly DbContextOptions<TwoButtonsContext> _dbOptions;
    private readonly ILogger<BotsController> _logger;
    public BotsController(BotsUnitOfWork uow, BotsManager botsManager, DbContextOptions<TwoButtonsContext> dbOptions, ILogger<BotsController> logger)
    {
      _uow = uow;
      _botsManager = botsManager;
      _dbOptions = dbOptions;
      _logger = logger;
    }

    [HttpPost("questions")]
    public async Task<IActionResult> GetQuestions([FromBody]GetQuestionsViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestions)}.Start");
      var result = await _uow.QuestionRepository.GetQuestions(vm.PageParams.Offset, vm.PageParams.Count);
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("questions/pattern")]
    public async Task<IActionResult> GetQuestionsByPattern([FromBody]GetGuestionsByPatternViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestionsByPattern)}.Start");
      var result = await _uow.QuestionRepository.GetQuestionsByPattern(vm.Pattern, vm.PageParams.Offset, vm.PageParams.Count);
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestionsByPattern)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("questions/id")]
    public async Task<IActionResult> GetQuestionsById([FromBody]GetQuestionByIdViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestionsById)}.Start");
      var result = await _uow.QuestionRepository.GetQuestionById(vm.QuestionId);
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetQuestionsById)}.End");
      return new OkResponseResult(result);
    }


    [HttpPost("count")]
    public async Task<IActionResult> GetBotsCount([FromBody]GetBotsCount vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetBotsCount)}.Start");
      var result = _uow.BotsRepository.GetBotsCount();
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(GetBotsCount)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("magic")]
    public async Task<IActionResult> Magic([FromBody]MagicViewModel vm)
    {
      if (vm.Code != "MySecretCode!123974_QQ")
        return new ResponseResult((int)HttpStatusCode.Forbidden, message: "You are hacker man)");
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(Magic)}.Start");
      await _botsManager.CreateTimer(_uow, _dbOptions,vm);
      _logger.LogInformation($"{nameof(BotsController)}.{nameof(Magic)}.End");
      return new OkResponseResult();
    }


  }
}
