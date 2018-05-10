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
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;

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

        [HttpPost("addComment")]
        public IActionResult AddComment([FromBody]AddCommentViewModel comment)
        {
            if (comment == null)
                return BadRequest($"Input parameter  is null");
            if (!CommentsWrapper.TryAddComment(_context, comment.UserId, comment.QuestionId, comment.CommentText, comment.PreviousCommnetId, out var commentId))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");
            return Ok(commentId);
            
        }

        [HttpPost("addCommentFeedback")]
        public IActionResult AddCommentFeedback([FromBody]AddCommentFeedbackViewModel commentFeedback)
        {
            if (commentFeedback == null)
                return BadRequest($"Input parameter {nameof(commentFeedback)} is null");
            if (CommentsWrapper.TryUpdateCommentFeedback(_context, commentFeedback.UserId, commentFeedback.CommentId, commentFeedback.FeedbackType))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("getComments")]
        public IActionResult GetComments([FromBody]GetCommentsViewModel comments)
        {
            if (comments == null)
                return BadRequest($"Input parameter {nameof(comments)} is null");
            if (CommentsWrapper.TryGetComments(_context, comments.UserId, comments.QuestionId, comments.Amount, out var comment))
                return Ok(comment);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


    }
}