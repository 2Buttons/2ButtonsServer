using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class PersonalQuestionsController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;

        public PersonalQuestionsController(TwoButtonsContext context)
        {
            _context = context;
        }


        [HttpPost("getAskedQuestions")]
        public IActionResult TryGetAskedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
        {
            if (userQuestions == null)
                return BadRequest($"Input parameter  is null");

            if (!UserQuestionsWrapper.TryGetAskedQuestions(_context, userQuestions.UserId, userQuestions.UserId, userQuestions.PageParams.Page,
                userQuestions.PageParams.Amount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<AskedQuestionsViewModel>();

            foreach (var question in userAskedQuestions)
            {
                GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
                    out var secondPhotos, out var comments);
                result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments));
            }
            return Ok(result);
        }


        [HttpPost("getLikedQuestions")]
        public IActionResult TryGetLikedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
        {
            if (userQuestions == null)
                return BadRequest($"Input parameter  is null");

            if (!UserQuestionsWrapper.TryGetLikedQuestions(_context, userQuestions.UserId, userQuestions.UserId, userQuestions.PageParams.Page,
                userQuestions.PageParams.Amount, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<LikedQuestionsViewModel>();

            foreach (var question in userAnsweredQuestions)
            {
                GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
                    out var secondPhotos, out var comments);
                result.Add(question.MapToLikedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments));
            }
            return Ok(result);
        }

        [HttpPost("getSavedQuestions")]
        public IActionResult TryGetSavedQuestions([FromBody] PersonalQuestionsViewModel userQuestions)
        {
            if (userQuestions == null)
                return BadRequest($"Input parameter  is null");

            if (!UserQuestionsWrapper.TryGetSavedQuestions(_context, userQuestions.UserId, userQuestions.PageParams.Page,
                userQuestions.PageParams.Amount, out var userFavoriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<SavedQuestionsViewModel>();


            foreach (var question in userFavoriteQuestions)
            {
                GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
                    out var secondPhotos, out var comments);
                result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments));
            }
            return Ok(result);
        }

      [HttpPost("getTopQuestions")]
      public IActionResult GetTopQuestions([FromBody] TopUserQuestions userQuestions)
      {
        if (userQuestions == null)
          return BadRequest($"Input parameter  is null");


        if (!UserQuestionsWrapper.TryGeTopQuestions(_context, userQuestions.UserId, userQuestions.IsOnlyNew, userQuestions.PageParams.Page,
          userQuestions.PageParams.Amount, userQuestions.TopAfterDate, out var userTopQuestions))
          return BadRequest("Something goes wrong. We will fix it!... maybe)))");

        var result = new List<TopQuestionsViewModel>();

        foreach (var question in userTopQuestions)
        {
          GetTagsAndPhotos(userQuestions.UserId, question.QuestionId, out var tags, out var firstPhotos,
            out var secondPhotos, out var comments);
          result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos, comments));
        }
        return Ok(result);
      }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
            out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
        {
            var commentsAmount = 10000;
            var photosAmount = 100;
            var minAge = 0;
            var maxAge = 100;
            var sex = 0;
            var city = string.Empty;

            if (!TagsWrapper.TryGetTags(_context, questionId, out tags))
                tags = new List<TagDb>();
            if (!ResultsWrapper.TryGetPhotos(_context, userId, questionId, 1, photosAmount, minAge, maxAge, sex, city, out firstPhotos))
                firstPhotos = new List<PhotoDb>();
            if (!ResultsWrapper.TryGetPhotos(_context, userId, questionId, 2, photosAmount, minAge, maxAge, sex, city, out secondPhotos))
                secondPhotos = new List<PhotoDb>();
            if (!CommentsWrapper.TryGetComments(_context, userId, questionId, commentsAmount, out comments))
                comments = new List<CommentDb>();
        }
    }
}