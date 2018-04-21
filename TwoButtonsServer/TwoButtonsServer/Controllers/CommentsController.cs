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
    [Produces("application/json")]
    [EnableCors("AllowAllOrigin")]
    //[Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly TwoButtonsContext _context;
        public CommentsController(TwoButtonsContext context)
        {
            _context = context;
        }

        // GET api/addComment/
        [HttpPost("addComment")]
        public IActionResult AddComment(int id, int questionId, string comment, int previousCommentId=0)
        {
            if (CommentsWrapper.TryAddComment(_context, id, questionId, comment, previousCommentId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/addCommentFeedback/
        [HttpPost("addCommentFeedback")]
        public IActionResult AddCommentFeedback(int id, int questionId, int feedback=0)
        {
            if (CommentsWrapper.TryAddCommentFeedback(_context, id, questionId, feedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/getComments/
        [HttpPost("getComments")]
        public IActionResult GetComments(int id, int questionId, int amount=0)
        {
            if (CommentsWrapper.TryGetComments(_context,id,questionId,amount, out var comment))
                return Ok(comment);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

    }
}