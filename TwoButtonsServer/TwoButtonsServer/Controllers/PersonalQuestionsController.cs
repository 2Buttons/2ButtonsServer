using System;
using System.Collections.Generic;
using System.Globalization;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsDatabase.Repositories;
using TwoButtonsServer.Extensions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("accountQuestions")]
  public class PersonalQuestionsController : Controller //To get user's posts
  {
    private readonly TwoButtonsContext _context;

    public PersonalQuestionsController(TwoButtonsContext context)
    {
      _context = context;
    }


    [HttpPost("asked")]
    public IActionResult GetAskedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      if (!UserQuestionsRepository.TryGetAskedQuestions(_context, userQuestions.UserId, userQuestions.UserId, userQuestions.PageParams.Offset,
          userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<AskedQuestionDb>(), out var userAskedQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<AskedQuestionsViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
            out var secondPhotos);
        result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    [HttpPost("recommended")]
    public IActionResult GetRecommendedQustions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      if (!UserQuestionsRepository.TryGetRecommendedQuestions(_context, userQuestions.UserId, userQuestions.UserId,
        userQuestions.PageParams.Offset,
        userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<RecommendedQuestionDb>(),
        out var recommendedQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

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
    public IActionResult GetLikedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      if (!UserQuestionsRepository.TryGetLikedQuestions(_context, userQuestions.UserId, userQuestions.PageParams.Offset,
          userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<LikedQuestionDb>(), out var userAnsweredQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

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
    public IActionResult GetSavedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      if (!UserQuestionsRepository.TryGetSavedQuestions(_context, userQuestions.UserId, userQuestions.PageParams.Offset,
          userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<SavedQuestionDb>(), out var userFavoriteQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

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
    public IActionResult GetTopQuestions([FromBody] TopDayQuestions questions)
    {
      if (questions == null)
        return BadRequest($"Input parameter  is null");
      var dateTime = questions.DeltaUnixTime == 0? DateTime.MinValue : DateTime.Now.AddSeconds(-questions.DeltaUnixTime);

      if (!UserQuestionsRepository.TryGeTopQuestions(_context, questions.UserId, questions.IsOnlyNew, questions.PageParams.Offset,
        questions.PageParams.Count, dateTime, questions.SortType.ToPredicate<TopQuestionDb>(), out var topQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<TopQuestionsViewModel>();

      foreach (var question in topQuestions)
      {
        GetTagsAndPhotos(questions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
            out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      if (!TagsRepository.TryGetTags(_context, questionId, out tags))
        tags = new List<TagDb>();
      if (!ResultsRepository.TryGetPhotos(_context, userId, questionId, 1, photosAmount, maxAge.WhenBorned(),minAge.WhenBorned(), sex, city, out firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!ResultsRepository.TryGetPhotos(_context, userId, questionId, 2, photosAmount, maxAge.WhenBorned(), minAge.WhenBorned(), sex, city, out secondPhotos))
        secondPhotos = new List<PhotoDb>();
    }
  }
}