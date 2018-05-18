using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;
using System.Linq;
using System.Security.Claims;
using CommonLibraries.Extensions;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsDatabase.Repositories;
using TwoButtonsServer.Extensions;
using TwoButtonsServer.ViewModels;

namespace TwoButtonsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("userQuestions")]
  public class UserPageQuestionsController : Controller //To get user's posts
  {
    private readonly TwoButtonsContext _context;

    public UserPageQuestionsController(TwoButtonsContext context)
    {
      _context = context;
    }

    //  [Authorize(Roles ="Guest,  User")]
    //[HttpPost("getUserAskedQuestions")]
    [HttpPost("asked")]
    public IActionResult GetUserAskedQuestions([FromBody] UserQuestionsViewModel userQuestions)
    {
      if (userQuestions == null || userQuestions.PageParams == null)
        return BadRequest($"Input parameter  is null");
      //  var u = User;

      if (!UserQuestionsRepository.TryGetUserAskedQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<UserAskedQuestionDb>(), out var userAskedQuestions))
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
      if (userQuestions == null || userQuestions.PageParams == null)
        return BadRequest($"Input parameter  is null");

      if (!UserQuestionsRepository.TryGetUserAnsweredQuestions(_context, userQuestions.UserId,
          userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count, out var userAnsweredQuestions))
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

      if (!UserQuestionsRepository.TryGetUserFavoriteQuestions(_context, userQuestions.UserId,
          userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<UserFavoriteQuestionDb>(), out var userFavoriteQuestions))
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

      if (!UserQuestionsRepository.TryGetUserCommentedQuestions(_context, userQuestions.UserId,
          userQuestions.UserPageId, userQuestions.PageParams.Offset, userQuestions.PageParams.Count, userQuestions.SortType.ToPredicate<UserCommentedQuestionDb>(), out var userCommentedQuestions))
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

      if (!TagsRepository.TryGetTags(_context, questionId, out tags))
        tags = new List<TagDb>();
      if (!ResultsRepository.TryGetPhotos(_context, userId, questionId, 1, photosAmount, maxAge.WhenBorned(), minAge.WhenBorned(), sex, city, out firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!ResultsRepository.TryGetPhotos(_context, userId, questionId, 2, photosAmount, maxAge.WhenBorned(), minAge.WhenBorned(), sex, city, out secondPhotos))
        secondPhotos = new List<PhotoDb>();
    }
  }
}