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
    private static Random Random { get; } = new Random();
    private ConnectionsHub Hub { get; }
    private ILogger<QuestionsController> Logger { get; }
    private QuestionsUnitOfWork MainDb { get; }
    private MediaConverter MediaConverter { get; }

    public QuestionsController(QuestionsUnitOfWork mainDb, ILogger<QuestionsController> logger, ConnectionsHub hub,
      MediaConverter mediaConverter)
    {
      MainDb = mainDb;
      Logger = logger;
      Hub = hub;
      MediaConverter = mediaConverter;
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
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(AddQuestion)}.Start");
      string externalUrl;

      if (!question.BackgroundUrl.IsNullOrEmpty())
      {
        if (MediaConverter.IsStandardBackground(question.BackgroundUrl))
          externalUrl = await Hub.Media.UploadBackgroundUrl(BackgroundType.Standard, question.BackgroundUrl);
        else externalUrl = await Hub.Media.UploadBackgroundUrl(BackgroundType.Custom, question.BackgroundUrl);
      }
      else
      {
        var urls = await Hub.Media.GetStandardBackgroundsUrl(BackgroundSizeType.Original);

        externalUrl = urls.Count < 1 ? null : urls[Random.Next(urls.Count)];
      }

      var questionId = await MainDb.Questions.AddQuestion(question.UserId, question.Condition, externalUrl,
        question.IsAnonymous, question.AudienceType, question.QuestionType, question.FirstOption,
        question.SecondOption);

      var badAddedTags = new List<string>();
      question.Tags = question.Tags.Select(x => x.Trim().ToUpper()).ToList();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (!await MainDb.Tags.AddTag(questionId, tag, i)) badAddedTags.Add(tag);
      }

      var notFoundIds = new List<int>();

      // var validIds = await _mainDb.UserQuestions.CheckIdsValid(question.RecommendedToIds);

      if (question.RecommendedToIds == null) question.RecommendedToIds = new List<int>();
      if (question.Tags == null) question.Tags = new List<string>();
      foreach (var id in question.RecommendedToIds)
        if (!await MainDb.UserQuestions.AddRecommendedQuestion(id, question.UserId, questionId))
          await Hub.Notifications.SendRecommendQuestionNotification(question.UserId, id, questionId, DateTime.UtcNow);
        else notFoundIds.Add(id);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(AddQuestion)}.End");
      if (badAddedTags.Count != 0)
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "Not all tags were inserted.",
          new {NotAddedTags = badAddedTags});
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
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(AddRecommendedQuestion)}.Start");
      var notFoundIds = new List<int>();

      // var validIds = await _mainDb.UserQuestions.CheckIdsValid(recommendedQuestion.UsersToId);

      foreach (var id in recommendedQuestion.UsersToId)
        if (await MainDb.UserQuestions.AddRecommendedQuestion(id, recommendedQuestion.UserFromId,
          recommendedQuestion.QuestionId))
          await Hub.Notifications.SendRecommendQuestionNotification(recommendedQuestion.UserFromId, id,
            recommendedQuestion.QuestionId, DateTime.UtcNow);
        else notFoundIds.Add(id);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(AddRecommendedQuestion)}.End");
      if (notFoundIds.Count > 0)
        return new ResponseResult((int) HttpStatusCode.NotModified, "Not all ids were found.",
          new {NotFoundIds = notFoundIds});
      return new ResponseResult((int) HttpStatusCode.Created, (object) "Recommended Question was added.");
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetQuestion([FromBody] GetQuestion getQuestion)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestion)}.Start");
      var question = await MainDb.Questions.FindQuestion(getQuestion.UserId, getQuestion.QuestionId);

      GetTagsAndPhotos(getQuestion.UserId, getQuestion.QuestionId, out var tags, out var firstPhotos,
        out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos, comments,
        getQuestion.BackgroundSizeType);
      await Hub.Monitoring.UpdateUrlMonitoring(getQuestion.UserId, UrlMonitoringType.OpensQuestionPage);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestion)}.End");
      return new OkResponseResult(result);
    }

    [HttpPost("get/statistic")]
    public async Task<IActionResult> GetQuestionFilteredStatistics([FromBody] GetQuestionFilteredStatistics statistics)
    {
      //TODO убрать
      if (statistics.UserId <= 0) return new BadResponseResult("UserId has to be more than 0");
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestionFilteredStatistics)}.Start");
      var result = await MainDb.Questions.GetQuestionStatistic(statistics.UserId, statistics.QuestionId,
        statistics.MinAge, statistics.MaxAge, statistics.SexType, statistics.City);
      await Hub.Monitoring.UpdateUrlMonitoring(statistics.UserId, UrlMonitoringType.FiltersQuestions);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestionFilteredStatistics)}.End");
      return new OkResponseResult("Question Statistic", result);
    }

    [HttpPost("get/statistic/users")]
    public async Task<IActionResult> GetQuestionFilteredStatisticsUsers(
      [FromBody] GetQuestionFilteredStatistics statistics)
    {
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestionFilteredStatisticsUsers)}.Start");
      var resultDto = await MainDb.Questions.GetQuestionStatistiсUsers(statistics.UserId, statistics.QuestionId,
        statistics.MinAge, statistics.MaxAge, statistics.SexType, statistics.City, statistics.PageParams.Offset,
        statistics.PageParams.Count);
      var result = new QuestionStatisticUserViewModel {Voters = new List<List<VoterUserViewModel>>()};
      foreach (var item in resultDto.Voters)
        result.Voters.Add(item.Select(x => new VoterUserViewModel
        {
          Age = x.Age,
          City = x.City,
          IsHeFollowed = x.IsHeFollowed,
          IsYouFollowed = x.IsYouFollowed,
          Login = x.Login,
          SexType = x.SexType,
          SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(x.OriginalAvatarUrl, AvatarSizeType.Small),
          UserId = x.UserId
        }).ToList());
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestionFilteredStatisticsUsers)}.End");
      return new OkResponseResult("Question Statistic -> Users", result);
    }

    [HttpPost("get/background")]
    public async Task<IActionResult> GetStandardBackgroundUrls([FromBody] GetQuestionBackground vm)
    {
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetStandardBackgroundUrls)}.Start");
      var result = await MainDb.Questions.GetBackground(vm.QuestionId);
      var url = MediaConverter.ToFullBackgroundurlUrl(result, vm.BackgroundSizeType);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetStandardBackgroundUrls)}.End");
      return new OkResponseResult("Background URL", new {Url = url});
    }

    [HttpPost("get/backgrounds/standard")]
    public async Task<IActionResult> GetStandardBackgroundUrls([FromBody] GetStandardBackgroundUrlsViewModel vm)
    {
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetStandardBackgroundUrls)}.Start");
      var result = await Hub.Media.GetStandardBackgroundsUrl(vm.BackgroundSizeType);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetStandardBackgroundUrls)}.End");
      return new OkResponseResult("Standard backgrounds",
        result.Select(x => MediaConverter.ToFullBackgroundurlUrl(x, vm.BackgroundSizeType)));
    }

    [HttpPost("get/backgrounds/custom")]
    public async Task<IActionResult> GetCustomQuestionBackgrounds([FromBody] GetCustomBackgroundUrlsViewModel user)
    {
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetCustomQuestionBackgrounds)}.Start");
      var result = await MainDb.Questions.GetCustomQuestionBackgrounds(user.UserId);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetCustomQuestionBackgrounds)}.End");
      return new OkResponseResult("Custom backgrounds",
        result.Select(x => MediaConverter.ToFullBackgroundurlUrl(x, user.BackgroundSizeType)).ToList());
    }

    [HttpPost("getByCommentId")]
    public async Task<IActionResult> GetQuestion([FromBody] GetQuestionByCommentId getQuestionByCommentId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestion)}.Start");
      var questionId = await MainDb.Questions.GetQuestionByCommentId(getQuestionByCommentId.CommentId);
      if (questionId <= 0)
        return new ResponseResult((int) HttpStatusCode.NotFound, "We can not find the question with this comment");

      var question = await MainDb.Questions.FindQuestion(getQuestionByCommentId.UserId, questionId);

      GetTagsAndPhotos(getQuestionByCommentId.UserId, questionId, out var tags, out var firstPhotos,
        out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos, comments,
        getQuestionByCommentId.BackgroundSizeType);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetQuestion)}.End");
      return new OkResponseResult(result);
    }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
    {
      var photosCount = 100;
      var commentsCount = 500;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = MainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = MainDb.Questions.GetPhotos(userId, questionId, 1, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos = MainDb.Questions.GetPhotos(userId, questionId, 2, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      comments = MainDb.Comments.GetComments(userId, questionId, 0, commentsCount).GetAwaiter().GetResult();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteQuestion([FromBody] QuestionIdViewModel questionId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(DeleteQuestion)}.Start");
      var result = await MainDb.Questions.DeleteQuestion(questionId.QuestionId);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(DeleteQuestion)}.End");
      return result
        ? new OkResponseResult((object) "Question was deleted.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Question was not deleted.");
    }

    [HttpPost("update/feedback")]
    public async Task<IActionResult> UpdateFeedback([FromBody] UpdateQuestionFeedbackViewModel feedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateFeedback)}.Start");
      var result =
        await MainDb.Questions.UpdateQuestionFeedback(feedback.UserId, feedback.QuestionId, feedback.FeedbackType);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateFeedback)}.End");
      return result
        ? new OkResponseResult((object) "Question's feedback was updated.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Question's feedback was not updated.");
    }

    [HttpPost("update/saved")]
    public async Task<IActionResult> UpdateSaved([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateSaved)}.Start");
      var result = await MainDb.Questions.UpdateSaved(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateSaved)}.End");
      return result
        ? new OkResponseResult((object) "Saves question was updated.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Save question was not updated.");
    }

    [HttpPost("update/favorites")]
    public async Task<IActionResult> UpdateFavorites([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateFavorites)}.Start");
      var result = await MainDb.Questions.UpdateFavorites(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateFavorites)}.End");
      return result
        ? new OkResponseResult((object) "Question's favorites was updated.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Question's favorites was not updated.");
    }

    [HttpPost("update/answer")]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateQuestionAnswerViewModel answer)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateAnswer)}.Start");
      var result = await MainDb.Questions.UpdateAnswer(answer.UserId, answer.QuestionId, answer.AnswerType);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateAnswer)}.End");
      return result
        ? new OkResponseResult((object) "Question's answer was updated.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Question's answer was not updated.");
    }

    [HttpPost("update/background/url")]
    public async Task<IActionResult> UpdateBackgroundViaUrl(
      [FromBody] UploadQuestionBackgroundViaUrlViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateBackgroundViaUrl)}.Start");
      string url;
      if (MediaConverter.IsStandardBackground(background.Url))
        url = await Hub.Media.UploadBackgroundUrl(BackgroundType.Standard, background.Url);
      else url = await Hub.Media.UploadBackgroundUrl(BackgroundType.Custom, background.Url);
      var isUpdated = !await MainDb.Questions.UpdateQuestionBackgroundUrl(background.QuestionId, url);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateBackgroundViaUrl)}.End");
      if (isUpdated) return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated",
        new {Url = MediaConverter.ToFullBackgroundurlUrl(url, background.BackgroundSizeType)});
    }

    [HttpPost("update/background/file")]
    public async Task<IActionResult> UpdateBackgroundViaFile(
      [FromBody] UploadQuestionBackgroundViaFileViewModel background)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateBackgroundViaFile)}.Start");
      var url = await Hub.Media.UploadBackgroundFile(BackgroundType.Custom, background.File);
      var result = !await MainDb.Questions.UpdateQuestionBackgroundUrl(background.QuestionId, url);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(UpdateBackgroundViaFile)}.End");
      if (result) return new ResponseResult((int) HttpStatusCode.NotModified, "We do not modify background.");
      return new OkResponseResult("Background was updated",
        new {Url = MediaConverter.ToFullBackgroundurlUrl(url, background.BackgroundSizeType)});
    }

    [HttpPost("voters")]
    public async Task<IActionResult> GetVoters([FromBody] GetVoters voters)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetVoters)}.Start");
      var answeredList = await MainDb.Questions.GetVoters(voters.UserId, voters.QuestionId, voters.PageParams.Offset,
        voters.PageParams.Count, voters.AnswerType, DateTime.UtcNow.AddYears(-voters.MaxAge),
        DateTime.UtcNow.AddYears(-voters.MinAge), voters.SexType, voters.SearchedLogin);
      Logger.LogInformation($"{nameof(QuestionsController)}.{nameof(GetVoters)}.End");
      return new OkResponseResult(answeredList.MapAnsweredListDbToViewModel(MediaConverter));
    }
  }
}