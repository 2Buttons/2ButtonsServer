using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    private readonly ConnectionsHub _hub;

    public TagsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub)
    {
      _mainDb = mainDb;
      _hub = hub;
    }

    [HttpPost]
    public async Task<IActionResult> GetTags([FromBody] GetTags getTag)
    {
      var result = await _mainDb.Tags.GetTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      return new OkResponseResult("Tags", result);
    }

    [HttpPost("popular")]
    public async Task<IActionResult> GetPopularTags([FromBody] GetTags getTag)
    {
      var result = await _mainDb.Tags.GetPopularTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      return new OkResponseResult("Popular Tags", result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTags([FromBody] AddTags tags)
    {
      var tagsEntities = new List<TagEntity>();
      foreach (var tag in tags.Tags)
      {
        tagsEntities.Add(new TagEntity {TagText = tag.Trim()});
      }
      var result = await _mainDb.Tags.AddTags(tagsEntities);
      return new OkResponseResult("Tags were added", result);
    }

  }
}