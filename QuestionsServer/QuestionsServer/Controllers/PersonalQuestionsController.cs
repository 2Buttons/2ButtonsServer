using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsData.Entities;
using QuestionsData.Entities.UserQuestions;
using QuestionsServer.Extensions;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;
using QuestionsServer.ViewModels.OutputParameters.UserQuestions;

namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("personal")]
  public class PersonalQuestionsController : Controller //To get user's posts
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public PersonalQuestionsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("asked")]
    public async Task<IActionResult> GetAskedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userASkedQuestions = await _mainDb.UserQuestions.GetAskedQuestions(userQuestions.UserId, userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<AskedQuestionDb>());

      var result = new List<AskedQuestionsViewModel>();

      foreach (var question in userASkedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return new OkResponseResult(result);
    }

    [HttpPost("recommended")]
    public async Task<IActionResult> GetRecommendedQustions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var recommendedQuestions = await _mainDb.UserQuestions.GetRecommendedQuestions(userQuestions.UserId,
        userQuestions.UserId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<RecommendedQuestionDb>());

      var result = new List<RecommendedQuestionViewModel>();

      foreach (var question in recommendedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return new OkResponseResult(result);
    }

    [HttpPost("liked")]
    public async Task<IActionResult> GetLikedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userAnsweredQuestions = await _mainDb.UserQuestions.GetLikedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<LikedQuestionDb>());

      var result = new List<LikedQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToLikedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return new OkResponseResult(result);
    }

    [HttpPost("saved")]
    public async Task<IActionResult> GetSavedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userFavoriteQuestions = await _mainDb.UserQuestions.GetSavedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<SavedQuestionDb>());

      var result = new List<SavedQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return new OkResponseResult(result);
    }

    [HttpPost("top")]
    public async Task<IActionResult> GetTopQuestions([FromBody] TopDayQuestions questions)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var dateTime = questions.DeltaUnixTime == 0
        ? DateTime.MinValue
        : DateTime.Now.AddSeconds(-questions.DeltaUnixTime);

      var topQuestions = await _mainDb.UserQuestions.GeTopQuestions(questions.UserId, questions.IsOnlyNew,
        questions.PageParams.Offset, questions.PageParams.Count, dateTime,
        questions.SortType.ToPredicate<TopQuestionDb>());

      var result = new List<TopQuestionsViewModel>();

      foreach (var question in topQuestions)
      {
        GetTagsAndPhotos(questions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
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

      var tagsTask = _mainDb.Tags.GetTags(questionId);
      var firstPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
      var secondPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);

      Task.WhenAll(tagsTask, firstPhotosTask, secondPhotosTask);
      tags = tagsTask.Result;
      firstPhotos = firstPhotosTask.Result;
      secondPhotos = secondPhotosTask.Result;
    }
  }
}