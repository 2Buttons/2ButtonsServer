using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using QuestionsServer.ViewModels.InputParameters;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;

namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("questions")]
  public class QuestionsController : Controller //Controller for a Question
  {
    private static readonly Random _random = new Random();
    private readonly ConnectionsHub _hub;
    private readonly ILogger<QuestionsController> _logger;
    private readonly QuestionsUnitOfWork _mainDb;

    public QuestionsController(QuestionsUnitOfWork mainDb, ILogger<QuestionsController> logger, ConnectionsHub hub)
    {
      _mainDb = mainDb;
      _logger = logger;
      _hub = hub;
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult("Questions Server");
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionViewModel question)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      string externalUrl;

      if (!question.BackgroundUrl.IsNullOrEmpty())
      {
        if (MediaConverter.IsStandardBackground(question.BackgroundUrl))
          externalUrl = await _hub.Media.UploadBackgroundUrl(BackgroundType.Standard, question.BackgroundUrl);
        else externalUrl = await _hub.Media.UploadBackgroundUrl(BackgroundType.Custom, question.BackgroundUrl);
      }
      else
      {
        var urls = await _hub.Media.GetStandardBackgroundsUrl(BackgroundSizeType.Original);

        externalUrl = urls.Count < 1 ? null : urls[_random.Next(urls.Count)];
      }

      var questionId = await _mainDb.Questions.AddQuestion(question.UserId, question.Condition, externalUrl,
        question.IsAnonymous, question.AudienceType, question.QuestionType, question.FirstOption,
        question.SecondOption);

      var badAddedTags = new List<string>();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (!await _mainDb.Tags.AddTag(questionId, tag, i)) badAddedTags.Add(tag);
      }

      var notFoundIds = new List<int>();

      // var validIds = await _mainDb.UserQuestions.CheckIdsValid(question.RecommendedToIds);

      if (question.RecommendedToIds == null) question.RecommendedToIds = new List<int>();
      if (question.Tags == null) question.Tags = new List<string>();
      foreach (var id in question.RecommendedToIds)
        if (!await _mainDb.UserQuestions.AddRecommendedQuestion(id, question.UserId, questionId))
          await _hub.Notifications.SendRecommendQuestionNotification(question.UserId, id, questionId, DateTime.UtcNow);
        else notFoundIds.Add(id);

      if (badAddedTags.Count != 0)
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "Not all tags were inserted.",
          new { NotAddedTags = badAddedTags });
      return new OkResponseResult(questionId);
    }

    [HttpPost("add/recommended")]
    public async Task<IActionResult> AddRecommendedQuestion(
      [FromBody] AddRecommendedQuestionViewModel recommendedQuestion)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (recommendedQuestion.UsersToId.Count < 1)
      {
        ModelState.AddModelError("UsersToId", "UsersToId is required and has to be more than 0");
        return new BadResponseResult(ModelState);
      }

      var notFoundIds = new List<int>();

      // var validIds = await _mainDb.UserQuestions.CheckIdsValid(recommendedQuestion.UsersToId);

      foreach (var id in recommendedQuestion.UsersToId)
        if (await _mainDb.UserQuestions.AddRecommendedQuestion(id, recommendedQuestion.UserFromId,
          recommendedQuestion.QuestionId))
          await _hub.Notifications.SendRecommendQuestionNotification(recommendedQuestion.UserFromId, id,
            recommendedQuestion.QuestionId, DateTime.UtcNow);
        else notFoundIds.Add(id);

      if (notFoundIds.Count > 0)
        return new ResponseResult((int)HttpStatusCode.NotModified, "Not all ids were found.",
          new { NotFoundIds = notFoundIds });
      return new ResponseResult((int)HttpStatusCode.Created, (object)"Recommended Question was added.");
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetQuestion([FromBody] GetQuestion getQuestion)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var question = await _mainDb.Questions.FindQuestion(getQuestion.UserId, getQuestion.QuestionId);

      GetTagsAndPhotos(getQuestion.UserId, getQuestion.QuestionId, out var tags, out var firstPhotos,
        out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments,
        getQuestion.BackgroundSizeType);
      await _hub.Monitoring.UpdateUrlMonitoring(getQuestion.UserId, UrlMonitoringType.OpensQuestionPage);
      return new OkResponseResult(result);
    }

    [HttpPost("get/statistic")]
    public async Task<IActionResult> GetQuestionFilteredStatistics([FromBody] GetQuestionFilteredStatistics statistics)
    {
      //TODO убрать
      if (statistics.UserId <= 0) return new BadResponseResult("UserId has to be more than 0");

      var result = await _mainDb.Questions.GetQuestionStatistic(statistics.UserId, statistics.QuestionId,
        statistics.MinAge, statistics.MaxAge, statistics.SexType, statistics.City);
      await _hub.Monitoring.UpdateUrlMonitoring(statistics.UserId, UrlMonitoringType.FiltersQuestions);
      return new OkResponseResult("Question Statistic", result);
    }

    [HttpPost("get/statistic/users")]
    public async Task<IActionResult> GetQuestionFilteredStatisticsUsers(
      [FromBody] GetQuestionFilteredStatistics statistics)
    {
      var result = await _mainDb.Questions.GetQuestionStatistiсUsers(statistics.UserId, statistics.QuestionId,
        statistics.MinAge, statistics.MaxAge, statistics.SexType, statistics.City, statistics.PageParams.Offset,
        statistics.PageParams.Count);
      return new OkResponseResult("Question Statistic -> Users", result);
    }

    [HttpPost("get/background")]
    public async Task<IActionResult> GetStandardBackgroundUrls([FromBody] GetQuestionBackground vm)
    {
      var result = await _mainDb.Questions.GetBackground(vm.QuestionId);
      var url = MediaConverter.ToFullBackgroundurlUrl(result, vm.BackgroundSizeType);
      return new OkResponseResult("Background URL", new {Url = url});
    }

    [HttpPost("get/backgrounds/standard")]
    public async Task<IActionResult> GetStandardBackgroundUrls([FromBody] GetStandardBackgroundUrlsViewModel vm)
    {
      var result = await _hub.Media.GetStandardBackgroundsUrl(vm.BackgroundSizeType);
      return new OkResponseResult("Standard backgrounds", result);
    }

    [HttpPost("get/backgrounds/custom")]
    public async Task<IActionResult> GetCustomQuestionBackgrounds([FromBody] GetCustomBackgroundUrlsViewModel user)
    {
      var result = await _mainDb.Questions.GetCustomQuestionBackgrounds(user.UserId);
      return new OkResponseResult("Custom backgrounds", result.Select(x=>MediaConverter.ToFullBackgroundurlUrl(x,user.BackgroundSizeType)).ToList());
    }

    [HttpPost("getByCommentId")]
    public async Task<IActionResult> GetQuestion([FromBody] GetQuestionByCommentId getQuestionByCommentId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var questionId = await _mainDb.Questions.GetQuestionByCommentId(getQuestionByCommentId.CommentId);
      if (questionId <= 0)
        return new ResponseResult((int) HttpStatusCode.NotFound, "We can not find the question with this comment");

      var question = await _mainDb.Questions.FindQuestion(getQuestionByCommentId.UserId, questionId);

      GetTagsAndPhotos(getQuestionByCommentId.UserId, questionId, out var tags, out var firstPhotos,
        out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments,
        getQuestionByCommentId.BackgroundSizeType);

      return new OkResponseResult(result);
    }

    private void GetTagsAndPhotos(long userId, long questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
    {
      var photosCount = 100;
      var commentsCount = 500;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = _mainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      comments = _mainDb.Comments.GetComments(userId, questionId, 0, commentsCount).GetAwaiter().GetResult();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteQuestion([FromBody] QuestionIdViewModel questionId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Questions.DeleteQuestion(questionId.QuestionId))
        return new OkResponseResult((object) "Question was deleted.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question was not deleted.");
    }

    [HttpPost("update/feedback")]
    public async Task<IActionResult> UpdateFeedback([FromBody] UpdateQuestionFeedbackViewModel feedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Questions.UpdateQuestionFeedback(feedback.UserId, feedback.QuestionId, feedback.FeedbackType))
        return new OkResponseResult((object) "Question's feedback was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question's feedback was not updated.");
    }

    [HttpPost("update/saved")]
    public async Task<IActionResult> UpdateSaved([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Questions.UpdateSaved(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites))
        return new OkResponseResult((object) "Saves question was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Save question was not updated.");
    }

    [HttpPost("update/favorites")]
    public async Task<IActionResult> UpdateFavorites([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Questions.UpdateFavorites(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites))
        return new OkResponseResult((object) "Question's favorites was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question's favorites was not updated.");
    }

    [HttpPost("update/answer")]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateQuestionAnswerViewModel answer)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (await _mainDb.Questions.UpdateAnswer(answer.UserId, answer.QuestionId, answer.AnswerType))
        return new OkResponseResult((object) "Question's answer was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question's answer was not updated.");
    }

    
    [HttpPost("update/background/url")]
    public async Task<IActionResult> UpdateBackgroundViaUrl(
      [FromBody] UploadQuestionBackgroundViaUrlViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      string url;
      if (MediaConverter.IsStandardBackground(background.Url))
        url = await _hub.Media.UploadBackgroundUrl(BackgroundType.Standard, background.Url);
      else url = await _hub.Media.UploadBackgroundUrl(BackgroundType.Custom, background.Url);
      if (!await _mainDb.Questions.UpdateQuestionBackgroundUrl(background.QuestionId, url))
        return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated",
        new {Url = MediaConverter.ToFullBackgroundurlUrl(url, background.BackgroundSizeType)});
    }

    [HttpPost("update/background/file")]
    public async Task<IActionResult> UpdateBackgroundViaFile(
      [FromBody] UploadQuestionBackgroundViaFileViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = await _hub.Media.UploadBackgroundFile(BackgroundType.Custom, background.File);
      if (!await _mainDb.Questions.UpdateQuestionBackgroundUrl(background.QuestionId, url))
        return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated",
        new {Url = MediaConverter.ToFullBackgroundurlUrl(url, background.BackgroundSizeType)});
    }

    [HttpPost("voters")]
    public async Task<IActionResult> GetVoters([FromBody] GetVoters voters)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var answeredList = await _mainDb.Questions.GetVoters(voters.UserId, voters.QuestionId, voters.PageParams.Offset,
        voters.PageParams.Count, voters.AnswerType, DateTime.UtcNow.AddYears(-voters.MaxAge),
        DateTime.UtcNow.AddYears(-voters.MinAge), voters.SexType, voters.SearchedLogin);

      return new OkResponseResult(answeredList.MapAnsweredListDbToViewModel());
    }
  }
}