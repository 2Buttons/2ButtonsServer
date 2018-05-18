using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;

namespace TwoButtonsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  //[Route("api/[controller]")]
  public class CommentsController : Controller
  {
    private readonly TwoButtonsUnitOfWork _mainDb;

    public CommentsController(TwoButtonsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("addComment")]
    public IActionResult AddComment([FromBody] AddCommentViewModel comment)
    {
      if (comment == null)
        return BadRequest($"Input parameter  is null");
      if (!_mainDb.Comments.TryAddComment(comment.UserId, comment.QuestionId, comment.CommentText,
        comment.PreviousCommnetId, out var commentId))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");
      return Ok(commentId);
    }

    [HttpPost("addCommentFeedback")]
    public IActionResult AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (commentFeedback == null)
        return BadRequest($"Input parameter {nameof(commentFeedback)} is null");
      if (_mainDb.Comments.TryUpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getComments")]
    public IActionResult GetComments([FromBody] GetCommentsViewModel comments)
    {
      if (comments == null)
        return BadRequest($"Input parameter {nameof(comments)} is null");
      if (_mainDb.Comments.TryGetComments(comments.UserId, comments.QuestionId, comments.Amount, out var comment))
        return Ok(comment);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }
  }
}