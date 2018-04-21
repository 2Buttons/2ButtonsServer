using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.UserQuestions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class WallController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;
        public WallController(TwoButtonsContext context)
        {
            _context = context;
        }

        [HttpGet("getPostsSampleActionResult")]
        public IActionResult GetPostsSampleActionResult(int id, int userId, int amount = 100)
        {
            return Ok("GetPosts" + " " + id + " " + userId + " " + amount);
        }



        [HttpGet("getPostsSample")]
        public string GetPostsSample(int id, int userId, int amount = 100)
        {
            return "GetPosts" + " " + id + " " + userId + " " + amount;
        }


        [HttpGet("getPosts")]
        public IActionResult GetPosts(int id, int userId, int amount = 100)
        {
            if (UserWrapper.TryGetPosts(_context, id, userId, amount, out var posts))
                return Ok(posts);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


        [HttpGet("getUserAskedQuestions")]
        public IActionResult GetUserAskedQuestions(int id, int userId, int amount = 100)
        {
            if (!UserQuestionsWrapper.TryGetUserAskedQuestions(_context, id, userId, amount, false,
                out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAskedQuestionsViewModel>();

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToUserAskedQuestionsViewModel(tags));
            }
            return Ok(result);

        }


        [HttpGet("getUserAnsweredQuestions")]
        public IActionResult GetUserAnsweredQuestions(int id, int userId, int amount = 100)
        {
            if (!UserQuestionsWrapper.TryGetUserAnsweredQuestions(_context, id, userId, amount, false, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAnsweredQuestionsViewModel>();

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToUserAnsweredQuestionsViewModel(tags));
            }
            return Ok(result);
        }

        [HttpGet("getUserFavoriteQuestions")]
        public IActionResult GetUserFavoriteQuestions(int id, int userId, int amount = 100)
        {
            if (!UserQuestionsWrapper.TryGetUserFavoriteQuestions(_context, id, userId, amount, true, out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserFavouriteQuestionsViewModel>();

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToUserFavouriteQuestionsViewModel(tags));
            }
            return Ok(result);
        }


        [HttpGet("getUserCommentedQuestions")]
        public IActionResult GetUserCommentedQuestions(int id, int userId, int amount = 100)
        {
            if (!UserQuestionsWrapper.TryGetUserCommentedQuestions(_context, id, userId, amount, out var userCommentedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserCommentedQuestionsViewModel>();

            foreach (var question in userCommentedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToUserCommentedQuestionsViewModel(tags));
            }
            return Ok(result);
        }


        [HttpGet("getTopQuestions")]
        public IActionResult GetTopQuestions(int id, DateTime topAfterDate, bool isOnlyNew = true, int amount = 100)
        {

            if (!UserQuestionsWrapper.TryGeTopQuestions(_context, id, isOnlyNew, amount, topAfterDate, out var userTopQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<TopQuestionsViewModel>();

            foreach (var question in userTopQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToTopQuestionsViewModel(tags));
            }
            return Ok(result);
        }
    }
}