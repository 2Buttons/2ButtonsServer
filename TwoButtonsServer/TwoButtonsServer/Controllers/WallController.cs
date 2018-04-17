using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

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
            if (UserPageWrapper.TryGetPosts(_context, id, userId, amount, out var posts))
                return Ok(posts);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

       
        [HttpGet("getUserAskedQuestions")]
        public IActionResult GetUserAskedQuestions(int id, int userId, int amount = 100)
        {
            if (QuestionsListWrapper.TryGetUserAskedQuestions(_context, id, userId, amount, false, out var userAskedQuestions))
                return Ok(userAskedQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        
        [HttpGet("getUserAnsweredQuestions")]
        public IActionResult GetUserAnsweredQuestions(int id, int userId, int amount = 100)
        {
            if (QuestionsListWrapper.TryGetUserAnsweredQuestions(_context, id, userId, amount, false, out var userAnsweredQuestions))
                return Ok(userAnsweredQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpGet("getUserFavoriteQuestions")]
        public IActionResult GetUserFavoriteQuestions(int id, int userId, int amount = 100)
        {
            if (QuestionsListWrapper.TryGetUserFavoriteQuestions(_context, id, userId, amount, true, out var userFavouriteQuestions))
                return Ok(userFavouriteQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

    
        [HttpGet("getUserCommentedQuestions")]
        public IActionResult GetUserCommentedQuestions(int id, int userId, int amount = 100)
        {
            if (QuestionsListWrapper.TryGetUserCommentedQuestions(_context, id, userId, amount, out var userCommentedQuestions))
                return Ok(userCommentedQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

    }
}