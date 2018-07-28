using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
using QuestionsData.Entities;
using QuestionsServer.ViewModels.InputParameters;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("questions/tags")]
  public class TagsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;
    private readonly ILogger<UserPageQuestionsController> _logger;

    public TagsController(QuestionsUnitOfWork mainDb,  ILogger<UserPageQuestionsController> logger)
    {
      _mainDb = mainDb;
      _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> GetTags([FromBody] GetTags getTag)
    {
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(GetTags)}.Start");
      var result = await _mainDb.Tags.GetTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(GetTags)}.End");
      return new OkResponseResult("Tags", result);
    }

    [HttpPost("popular")]
    public async Task<IActionResult> GetPopularTags([FromBody] GetTags getTag)
    {
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(GetPopularTags)}.Start");
      var result = await _mainDb.Tags.GetPopularTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(GetPopularTags)}.End");
      return new OkResponseResult("Popular Tags", result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTags([FromBody] AddTags tags)
    {
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(AddTags)}.Start");
      var tagsEntities = new List<TagEntity>();
      foreach (var tag in tags.Tags)
      {
        tagsEntities.Add(new TagEntity {Text = tag.Trim()});
      }
      var result = await _mainDb.Tags.AddTags(tagsEntities);
      _logger.LogInformation($"{nameof(TagsController)}.{nameof(AddTags)}.End");
      return new OkResponseResult("Tags were added", result);
    }

  }
}