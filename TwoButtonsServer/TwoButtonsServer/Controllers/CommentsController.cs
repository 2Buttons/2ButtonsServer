using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels.InputParameters;

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
        public IActionResult AddComment([FromBody]AddCommentViewModel comment)
        {
            if (CommentsWrapper.TryAddComment(_context, comment.UserId, comment.QuestionId, comment.Comment, comment.PreviousCommnetId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/addCommentFeedback/
        [HttpPost("addCommentFeedback")]
        public IActionResult AddCommentFeedback([FromBody]AddCommentFeedbackViewModel commentFeedback)
        {
            if (CommentsWrapper.TryAddCommentFeedback(_context, commentFeedback.UserId, commentFeedback.QuestionId, commentFeedback.Feedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/getComments/
        [HttpPost("getComments")]
        public IActionResult GetComments([FromBody]GetCommentsViewModel comments)
        {
            if (CommentsWrapper.TryGetComments(_context, comments.UserId, comments.QuestionId, comments.Amount, out var comment))
                return Ok(comment);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


    }
}