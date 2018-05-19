using System.Threading.Tasks;
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
    public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel comment)
    {
      if (comment == null)
        return BadRequest($"Input parameter  is null");
      var comentId = await _mainDb.Comments.AddComment(comment.UserId, comment.QuestionId, comment.CommentText,
        comment.PreviousCommnetId);
      return Ok(comentId);
    }

    [HttpPost("addCommentFeedback")]
    public async Task<IActionResult> AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (commentFeedback == null)
        return BadRequest($"Input parameter {nameof(commentFeedback)} is null");
      if (await _mainDb.Comments.UpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getComments")]
    public async Task<IActionResult> GetComments([FromBody] GetCommentsViewModel commentsVm)
    {
      if (commentsVm == null)
        return BadRequest($"Input parameter {nameof(commentsVm)} is null");
      var comments = await (_mainDb.Comments.GetComments(commentsVm.UserId, commentsVm.QuestionId, commentsVm.Amount));
        return Ok(comments);
    }
  }
}