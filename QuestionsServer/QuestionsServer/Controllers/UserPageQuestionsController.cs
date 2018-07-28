using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
using QuestionsData.DTO;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsData.Queries.UserQuestions;
using QuestionsServer.Extensions;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;
using QuestionsServer.ViewModels.OutputParameters.UserQuestions;

namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("questions/user")]
  public class UserPageQuestionsController : Controller //To get user's posts
  {
    private readonly QuestionsUnitOfWork _mainDb;
    private readonly ConnectionsHub _hub;
    private readonly ILogger<UserPageQuestionsController> _logger;

    public UserPageQuestionsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub, ILogger<UserPageQuestionsController> logger)
    {
      _mainDb = mainDb;
      _hub = hub;
      _logger = logger;
    }

    //[Authorize(Roles ="Guest,  User")]
    [HttpPost("asked")]
    public async Task<IActionResult> GetUserAskedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAskedQuestions)}.Start");
      var userAskedQuestions = await _mainDb.UserQuestions.GetUserAskedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserAskedQuestionDb>());
      var result = new List<UserAskedQuestionsViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAskedQuestionsViewModel(tags, firstPhotos, secondPhotos, userQuestions.BackgroundSizeType));
      }

      await _hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserAsked);
      _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAskedQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("answered")]
    public async Task<IActionResult> GetUserAnsweredQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAnsweredQuestions)}.Start");
      var userAnsweredQuestions = await _mainDb.UserQuestions.GetUserAnsweredQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count);

      var result = new List<UserAnsweredQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos, userQuestions.BackgroundSizeType));
      }
      await _hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserAnswered);
        _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAnsweredQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("favorite")]
    public async Task<IActionResult> GetUserFavoriteQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
        _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserFavoriteQuestions)}.Start");
      var userFavoriteQuestions = await _mainDb.UserQuestions.GetUserFavoriteQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserFavoriteQuestionDb>());

      var result = new List<UserFavoriteQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos, userQuestions.BackgroundSizeType));
      }
      await _hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserFavorite);
        _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserFavoriteQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("commented")]
    public async Task<IActionResult> GetUserCommentedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
        _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserCommentedQuestions)}.Start");
      var userCommentedQuestions = await _mainDb.UserQuestions.GetUserCommentedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserCommentedQuestionDb>());

      var result = new List<UserCommentedQuestionsViewModel>();

      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos, userQuestions.BackgroundSizeType));
      }
      await _hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserCommented);
        _logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserCommentedQuestions)}.End");
      return new OkResponseResult(result);
    }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosCount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = _mainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
    }
  }
}