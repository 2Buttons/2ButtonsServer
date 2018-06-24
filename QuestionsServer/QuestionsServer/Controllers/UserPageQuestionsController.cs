using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
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

    public UserPageQuestionsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    //[Authorize(Roles ="Guest,  User")]
    [HttpPost("asked")]
    public async Task<IActionResult> GetUserAskedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userAskedQuestions = await _mainDb.UserQuestions.GetUserAskedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserAskedQuestionDb>());
      var result = new List<UserAskedQuestionsViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }

      MonitoringServerConnectionService.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserAsked);
      return new OkResponseResult(result);
    }

    [HttpPost("answered")]
    public async Task<IActionResult> GetUserAnsweredQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userAnsweredQuestions = await _mainDb.UserQuestions.GetUserAnsweredQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count);

      var result = new List<UserAnsweredQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      MonitoringServerConnectionService.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserAnswered);
      return new OkResponseResult(result);
    }

    [HttpPost("favorite")]
    public async Task<IActionResult> GetUserFavoriteQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userFavoriteQuestions = await _mainDb.UserQuestions.GetUserFavoriteQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserFavoriteQuestionDb>());

      var result = new List<UserFavoriteQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      MonitoringServerConnectionService.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserFavorite);
      return new OkResponseResult(result);
    }

    [HttpPost("commented")]
    public async Task<IActionResult> GetUserCommentedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userCommentedQuestions = await _mainDb.UserQuestions.GetUserCommentedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserCommentedQuestionDb>());

      var result = new List<UserCommentedQuestionsViewModel>();

      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      MonitoringServerConnectionService.UpdateUrlMonitoring(userQuestions.UserId,
        CommonLibraries.UrlMonitoringType.GetsQuestionsUserCommented);
      return new OkResponseResult(result);
    }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      //var tagsTask = _mainDb.Tags.GetTags(questionId);
      //var firstPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
      //  minAge.WhenBorned(), sex, city);
      //var secondPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
      //  minAge.WhenBorned(), sex, city);

      //Task.WhenAll(tagsTask, firstPhotosTask, secondPhotosTask);
      //tags = tagsTask.Result;
      //firstPhotos = firstPhotosTask.Result;
      //secondPhotos = secondPhotosTask.Result;


      tags = _mainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
    }
  }
}