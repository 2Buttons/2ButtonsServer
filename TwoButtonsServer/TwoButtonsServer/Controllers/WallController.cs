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
            if (!QuestionsListWrapper.TryGetUserAskedQuestions(_context, id, userId, amount, false,
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
            if (!QuestionsListWrapper.TryGetUserAnsweredQuestions(_context, id, userId, amount, false, out var userAnsweredQuestions))
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
            if (!QuestionsListWrapper.TryGetUserFavoriteQuestions(_context, id, userId, amount, true, out var userFavouriteQuestions))
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
            if (!QuestionsListWrapper.TryGetUserCommentedQuestions(_context, id, userId, amount, out var userCommentedQuestions))
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
    }
}