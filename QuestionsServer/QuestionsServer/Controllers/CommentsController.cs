using System;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("comments")]
  public class CommentsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public CommentsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel comment)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var commentId = await _mainDb.Comments.AddComment(comment.UserId, comment.QuestionId, comment.CommentText,
        comment.PreviousCommnetId);
      if (commentId < 0)
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not create comment");
      //if (comment.PreviousCommnetId > 0) NotificationServerHelper.SendCommentNotification(comment.UserId, comment.QuestionId, commentId, DateTime.UtcNow);
      return new ResponseResult((int)HttpStatusCode.Created, new { CommentId = commentId });
    }

    [HttpPost("update/feedback")]
    public async Task<IActionResult> AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (await _mainDb.Comments.UpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType)) return new OkResponseResult(new { IsFeedbackUpdated = true });
      return new ResponseResult((int)HttpStatusCode.NotModified, new { IsFeedbackUpdated = false });
    }

    [HttpPost]
    public async Task<IActionResult> GetComments([FromBody] GetCommentsViewModel commentsVm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var comments = await _mainDb.Comments.GetComments(commentsVm.UserId, commentsVm.QuestionId, commentsVm.Amount);
      return new OkResponseResult(comments);
    }
  }
}