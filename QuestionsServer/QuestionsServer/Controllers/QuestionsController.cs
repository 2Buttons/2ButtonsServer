using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost("{questionId:int}")]
    public async Task<IActionResult> GetQuestion(int qiestionId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      var question = await _mainDb.Questions.FindQuestion(userId, qiestionId);

      GetTagsAndPhotos(userId, qiestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);

      return new OkResponseResult(result);
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetQuestion([FromBody] QuestionIdViewModel inputQuestion)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var question = await _mainDb.Questions.FindQuestion(inputQuestion.UserId, inputQuestion.QuestionId);

      GetTagsAndPhotos(inputQuestion.UserId, inputQuestion.QuestionId, out var tags, out var firstPhotos,
        out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
      _hub.Monitoring.UpdateUrlMonitoring(inputQuestion.UserId, UrlMonitoringType.OpensQuestionPage);
      return new OkResponseResult(result);
    }

    [HttpPost("get/statistic")]
    public async Task<IActionResult> GetQuestionFilteredStatistics([FromBody] GetQuestionFilteredStatistics statistics)
    {
      //TODO убрать
      if (statistics.UserId <= 0) return new BadResponseResult("UserId has to be more than 0");

      var result = await _mainDb.Questions.GetQuestionStatistic(statistics.UserId, statistics.QuestionId,
        statistics.MinAge, statistics.MaxAge, statistics.SexType, statistics.City);
      _hub.Monitoring.UpdateUrlMonitoring(statistics.UserId, UrlMonitoringType.FiltersQuestions);
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

    [HttpPost("get/backgrounds/standard")]
    public async Task<IActionResult> GetStandardQuestionBackgrounds()
    {
      var result = await _hub.Media.GetStandardBackgroundsUrl();
      return new OkResponseResult("Standard backgrounds", result);
    }

    [HttpPost("get/backgrounds/custom")]
    public async Task<IActionResult> GetCustomQuestionBackgrounds([FromBody] UserIdViewModel user)
    {
      var result = await _mainDb.Questions.GetCustomQuestionBackgrounds(user.UserId);
      return new OkResponseResult("Custom backgrounds", result);
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

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);

      return new OkResponseResult(result);
    }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
    {
      var photosAmount = 100;
      var commentsAmount = 500;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = _mainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      comments = _mainDb.Comments.GetComments(userId, questionId, commentsAmount).GetAwaiter().GetResult();
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionViewModel question)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var backgroundLink = _hub.Media.StandardBackground();
      if (!question.BackgroundImageLink.IsNullOrEmpty())
      {
        var externalUrl = await _hub.Media.UploadBackgroundUrl(question.BackgroundImageLink);

        if (!externalUrl.IsNullOrEmpty()) backgroundLink = externalUrl;
      }

      var questionId = await _mainDb.Questions.AddQuestion(question.UserId, question.Condition, backgroundLink,
        question.IsAnonymity ? 1 : 0, question.AudienceType, question.QuestionType, question.FirstOption,
        question.SecondOption);

      var badAddedTags = new List<string>();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (!await _mainDb.Tags.AddTag(questionId, tag, i)) badAddedTags.Add(tag);
      }

      var notFoundIds = new List<int>();

     // var validIds = await _mainDb.UserQuestions.CheckIdsValid(question.RecommendedToIds);

      foreach (var id in question.RecommendedToIds)
      {
        if (!await _mainDb.UserQuestions.AddRecommendedQuestion(id, question.UserId,
              questionId))
          _hub.Notifications.SendRecommendQuestionNotification(question.UserId, id,
            questionId, DateTime.UtcNow);
        else notFoundIds.Add(id);
      }

      if (badAddedTags.Count != 0)
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "Not all tags were inserted.",
          new {NotAddedTags = badAddedTags});
      return new OkResponseResult(questionId);
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
        return new OkResponseResult((object) "Question's favourites was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question's favourites was not updated.");
    }

    [HttpPost("update/answer")]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateQuestionAnswerViewModel answer)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (await _mainDb.Questions.UpdateAnswer(answer.UserId, answer.QuestionId, answer.AnswerType))
        return new OkResponseResult((object) "Question's answer was updated.");
      return new ResponseResult((int) HttpStatusCode.NotModified, "Question's answer was not updated.");
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
      {
        if (await _mainDb.UserQuestions.AddRecommendedQuestion(id, recommendedQuestion.UserFromId,
          recommendedQuestion.QuestionId))
          _hub.Notifications.SendRecommendQuestionNotification(recommendedQuestion.UserFromId, id,
            recommendedQuestion.QuestionId, DateTime.UtcNow);
        else notFoundIds.Add(id);
      }

      if (notFoundIds.Count > 0)
        return new ResponseResult((int) HttpStatusCode.NotModified, "Not all ids were found.",
          new {NotFoundIds = notFoundIds});
      return new ResponseResult((int) HttpStatusCode.Created, (object) "Recommended Question was added.");
    }

    [HttpPost("update/background/link")]
    public async Task<IActionResult> UpdateBackgroundViaLink(
      [FromBody] UploadQuestionBackgroundViaLinkViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var url = await _hub.Media.UploadBackgroundUrl(background.Url);
      if (!await _mainDb.Questions.UpdateQuestionBackgroundLink(background.QuestionId, url))
        return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated", new {Url = url});
    }

    [HttpPost("update/background/file")]
    public async Task<IActionResult> UpdateBackgroundViaFile(
      [FromBody] UploadQuestionBackgroundViaFileViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var url = await _hub.Media.UploadBackgroundFile(background.File);
      if (!await _mainDb.Questions.UpdateQuestionBackgroundLink(background.QuestionId, url))
        return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated", new {Url = url});
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