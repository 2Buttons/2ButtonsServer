using System.Collections.Generic;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsServer.Extensions;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("userQuestions")]
  public class UserPageQuestionsController : Controller //To get user's posts
  {
    private readonly TwoButtonsUnitOfWork _mainDb;

    public UserPageQuestionsController(TwoButtonsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    //  [Authorize(Roles ="Guest,  User")]
    //[HttpPost("getUserAskedQuestions")]
    [HttpPost("asked")]
    public IActionResult GetUserAskedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (userQuestions?.PageParams == null)
        return BadRequest($"Input parameter  is null");
      //  var u = User;

      if (!_mainDb.UserQuestions.TryGetUserAskedQuestions(userQuestions.UserId, userQuestions.UserPageId,
        userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserAskedQuestionDb>(), out var userAskedQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");
      // int o = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var result = new List<UserAskedQuestionsViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    //[HttpPost("getUserAnsweredQuestions")]
    [HttpPost("answered")]
    public IActionResult GetUserAnsweredQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (userQuestions?.PageParams == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.UserQuestions.TryGetUserAnsweredQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        out var userAnsweredQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<UserAnsweredQuestionsViewModel>();

      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }

    //[HttpPost("getUserFavoriteQuestions")]
    [HttpPost("favorite")]
    public IActionResult GetUserFavoriteQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (userQuestions == null || userQuestions.PageParams == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.UserQuestions.TryGetUserFavoriteQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserFavoriteQuestionDb>(), out var userFavoriteQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<UserFavoriteQuestionsViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos));
      }
      return Ok(result);
    }


    //[HttpPost("getUserCommentedQuestions")]
    [HttpPost("commented")]
    public IActionResult GetUserCommentedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (userQuestions == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.UserQuestions.TryGetUserCommentedQuestions(userQuestions.UserId,
        userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count,
        userQuestions.SortType.ToPredicate<UserCommentedQuestionDb>(), out var userCommentedQuestions))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var result = new List<UserCommentedQuestionsViewModel>();

      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        result.Add(question.MapToUserCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos));
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

      if (!_mainDb.Tags.TryGetTags(questionId, out tags))
        tags = new List<TagDb>();
      if (!_mainDb.Questions.TryGetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!_mainDb.Questions.TryGetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out secondPhotos))
        secondPhotos = new List<PhotoDb>();
    }
  }
}