using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Entities.Main;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("questions/tags")]
  public class TagsController : Controller
  {
    private QuestionsUnitOfWork MainDb { get; }
    private ILogger<UserPageQuestionsController> Logger { get; }

    public TagsController(QuestionsUnitOfWork mainDb, ILogger<UserPageQuestionsController> logger)
    {
      MainDb = mainDb;
      Logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> GetTags([FromBody] GetTags getTag)
    {
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(GetTags)}.Start");
      var result = await MainDb.Tags.GetTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(GetTags)}.End");
      return new OkResponseResult("Tags", result);
    }

    [HttpPost("popular")]
    public async Task<IActionResult> GetPopularTags([FromBody] GetTags getTag)
    {
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(GetPopularTags)}.Start");
      var result = await MainDb.Tags.GetPopularTags(getTag.PageParams.Offset, getTag.PageParams.Count);
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(GetPopularTags)}.End");
      return new OkResponseResult("Popular Tags", result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTags([FromBody] AddTags tags)
    {
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(AddTags)}.Start");
      var tagsEntities = new List<TagEntity>();
      foreach (var tag in tags.Tags) tagsEntities.Add(new TagEntity {Text = tag.Trim()});
      var result = await MainDb.Tags.AddTags(tagsEntities);
      Logger.LogInformation($"{nameof(TagsController)}.{nameof(AddTags)}.End");
      return new OkResponseResult("Tags were added", result);
    }
  }
}