using System;
using System.Collections.Generic;
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
  //[Route("api/[controller]")]
  [Route("questions/personal")]
  public class PersonalQuestionsController : Controller //To get user's posts
  {
    private QuestionsUnitOfWork MainDb { get; }
    private ConnectionsHub Hub { get; }
    private ILogger<PersonalQuestionsController> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public PersonalQuestionsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub,
      ILogger<PersonalQuestionsController> logger, MediaConverter mediaConverter)
    {
      MainDb = mainDb;
      Hub = hub;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    [HttpPost("asked")]
    public async Task<IActionResult> GetAskedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetAskedQuestions)}.Start");
      var userASkedQuestions = await MainDb.UserQuestions.GetAskedQuestions(userQuestions.UserId, userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<AskedQuestionDb>());

      var result = new List<AskedQuestionsViewModel>();

      foreach (var question in userASkedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToAskedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }

      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsPersonalAsked);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetAskedQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("recommended")]
    public async Task<IActionResult> GetRecommendedQustions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetRecommendedQustions)}.Start");
      var recommendedQuestions = await MainDb.UserQuestions.GetRecommendedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<RecommendedQuestionDb>());

      var result = new List<RecommendedQuestionViewModel>();

      foreach (var question in recommendedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToRecommendedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId,
        UrlMonitoringType.GetsQuestionsPersonalRecommended);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetRecommendedQustions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("selected")]
    public async Task<IActionResult> GetChosenQustions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetChosenQustions)}.Start");
      var chosenQuestions = await MainDb.UserQuestions.GetSelectedQuestions(userQuestions.UserId, userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count);

      var result = new List<SelectedQuestionsViewModel>();

      foreach (var question in chosenQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToChosenQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsPersonalChosen);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetChosenQustions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("liked")]
    public async Task<IActionResult> GetLikedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetLikedQuestions)}.Start");
      var userAnsweredQuestions = await MainDb.UserQuestions.GetLikedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<LikedQuestionDb>());

      var result = new List<LikedQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToLikedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsPersonalLiked);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetLikedQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("saved")]
    public async Task<IActionResult> GetSavedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetSavedQuestions)}.Start");
      var userFavoriteQuestions = await MainDb.UserQuestions.GetSavedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<SavedQuestionDb>());

      var result = new List<SavedQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToSavedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          userQuestions.BackgroundSizeType));
      }
      await Hub.Monitoring.UpdateUrlMonitoring(userQuestions.UserId, UrlMonitoringType.GetsQuestionsPersonalSaved);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetSavedQuestions)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("top")]
    public async Task<IActionResult> GetTopQuestions([FromBody] TopDayQuestions questions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetTopQuestions)}.Start");
      var dateTime = questions.DeltaUnixTime == 0
        ? DateTime.MinValue
        : DateTime.Now.AddSeconds(-questions.DeltaUnixTime);

      var topQuestions = await MainDb.UserQuestions.GeTopQuestions(questions.UserId, questions.IsOnlyNew,
        questions.PageParams.Offset, questions.PageParams.Count, dateTime);

      //var topQuestions = await _mainDb.UserQuestions.GeTopQuestions(questions.UserId, questions.IsOnlyNew,
      //  questions.PageParams.Offset, questions.PageParams.Count, dateTime);

      var result = new List<TopQuestionsViewModel>();

      foreach (var question in topQuestions)
      {
        GetTagsAndPhotos(questions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToTopQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
          questions.BackgroundSizeType));
      }

      const int unixDay = 24 * 60 * 60;
      const int unixWeek = 7 * 24 * 60 * 60;
      const int unixMonth = 30 * 24 * 60 * 60;

      switch (questions.DeltaUnixTime)
      {
        case unixDay:
          await Hub.Monitoring.UpdateUrlMonitoring(questions.UserId, UrlMonitoringType.GetsQuestionsPersonalDayTop);
          break;
        case unixWeek:
          await Hub.Monitoring.UpdateUrlMonitoring(questions.UserId, UrlMonitoringType.GetsQuestionsPersonalWeekTop);
          break;
        case unixMonth:
          await Hub.Monitoring.UpdateUrlMonitoring(questions.UserId, UrlMonitoringType.GetsQuestionsPersonalMonthTop);
          break;
        default:
          await Hub.Monitoring.UpdateUrlMonitoring(questions.UserId, UrlMonitoringType.GetsQuestionsPersonalAllTimeTop);
          break;
      }
      Logger.LogInformation($"{nameof(PersonalQuestionsController)}.{nameof(GetTopQuestions)}.End");
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