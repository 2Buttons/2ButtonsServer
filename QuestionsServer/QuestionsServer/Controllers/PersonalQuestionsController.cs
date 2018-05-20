using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
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
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      var userASkedQuestions = await _mainDb.UserQuestions.GetAskedQuestions(userQuestions.UserId, userQuestions.UserId,
        userQuestions.PageParams.Offset,
        userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<AskedQuestionDb>());
       // return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<AskedQuestionsViewModel>();

      foreach (var question in userASkedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
            out var secondPhotos);
        result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    [HttpPost("recommended")]
    public async Task<IActionResult> GetRecommendedQustions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      var recommendedQuestions = await _mainDb.UserQuestions.GetRecommendedQuestions(userQuestions.UserId,
        userQuestions.UserId,
        userQuestions.PageParams.Offset,
        userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<RecommendedQuestionDb>());
        //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<RecommendedQuestionViewModel>();

      foreach (var question in recommendedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }



    [HttpPost("liked")]
    public async Task<IActionResult> GetLikedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      var userAnsweredQuestions = await _mainDb.UserQuestions.GetLikedQuestions(userQuestions.UserId,
        userQuestions.PageParams.Offset,
        userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<LikedQuestionDb>());
        //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<LikedQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
            out var secondPhotos);
        result.Add(question.MapToLikedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }

    [HttpPost("saved")]
    public async Task<IActionResult> GetSavedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      var userFavoriteQuestions = await _mainDb.UserQuestions.GetSavedQuestions(userQuestions.UserId, userQuestions.PageParams.Offset,
          userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<SavedQuestionDb>());
        //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<SavedQuestionsViewModel>();


      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
            out var secondPhotos);
        result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }

    [HttpPost("top")]
    public async Task<IActionResult> GetTopQuestions([FromBody] TopDayQuestions questions)
    {
      if (questions == null)
        return BadRequest($"Input parameter  is null");
      var dateTime = questions.DeltaUnixTime == 0? DateTime.MinValue : DateTime.Now.AddSeconds(-questions.DeltaUnixTime);

      var topQuestions = await _mainDb.UserQuestions.GeTopQuestions(questions.UserId, questions.IsOnlyNew,
        questions.PageParams.Offset,
        questions.PageParams.Count, dateTime, questions.SortType.ToPredicate<TopQuestionDb>());
        //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<TopQuestionsViewModel>();

      foreach (var question in topQuestions)
      {
        GetTagsAndPhotos(questions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    private  void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
            out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      var tagsTask= _mainDb.Tags.GetTags(questionId);
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