﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
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
    private QuestionsUnitOfWork MainDb { get; }
    private ConnectionsHub Hub { get; }
    private ILogger<UserPageQuestionsController> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public UserPageQuestionsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub,
      ILogger<UserPageQuestionsController> logger, MediaConverter mediaConverter)
    {
      MainDb = mainDb;
      Hub = hub;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    //[Authorize(Roles ="Guest,  User")]
    [HttpPost("asked")]
    public async Task<IActionResult> GetUserAskedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAskedQuestions)}.Start");
      var userAskedQuestions = await MainDb.UserQuestions.GetUserAskedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserAskedQuestionDb>());
      var result = new List<UserAskedQuestionsViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAskedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }

      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsUserAsked);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAskedQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("answered")]
    public async Task<IActionResult> GetUserAnsweredQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAnsweredQuestions)}.Start");
      var userAnsweredQuestions = await MainDb.UserQuestions.GetUserAnsweredQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count);

      var result = new List<UserAnsweredQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAnsweredQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsUserAnswered);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserAnsweredQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("favorite")]
    public async Task<IActionResult> GetUserFavoriteQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserFavoriteQuestions)}.Start");
      var userFavoriteQuestions = await MainDb.UserQuestions.GetUserFavoriteQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserFavoriteQuestionDb>());

      var result = new List<UserFavoriteQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserFavoriteQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsUserFavorite);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserFavoriteQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("commented")]
    public async Task<IActionResult> GetUserCommentedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserCommentedQuestions)}.Start");
      var userCommentedQuestions = await MainDb.UserQuestions.GetUserCommentedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserCommentedQuestionDb>());

      var result = new List<UserCommentedQuestionsViewModel>();

      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserCommentedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsUserCommented);
      Logger.LogInformation($"{nameof(UserPageQuestionsController)}.{nameof(GetUserCommentedQuestions)}.End");
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

      tags = MainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = MainDb.Questions.GetPhotos(userId, questionId, 1, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = MainDb.Questions.GetPhotos(userId, questionId, 2, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
    }
  }
}