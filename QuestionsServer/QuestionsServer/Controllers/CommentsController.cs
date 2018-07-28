using System;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("questions/comments")]
  public class CommentsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;
    private readonly ConnectionsHub _hub;
    private readonly ILogger<ComplaintsController> _logger;

    public CommentsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub, ILogger<ComplaintsController> logger)
    {
      _mainDb = mainDb;
      _hub = hub;
      _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel comment)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (comment.PreviousCommentId == 0) comment.PreviousCommentId = null;
      var commentId = await _mainDb.Comments.AddComment(comment.UserId, comment.QuestionId, comment.Text,
        comment.PreviousCommentId);
      if (commentId < 0)
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not create comment");
      if (comment.PreviousCommentId > 0) await _hub.Notifications.SendCommentNotification(comment.UserId, comment.QuestionId, commentId, DateTime.UtcNow);
      return new ResponseResult((int)HttpStatusCode.Created, new { CommentId = commentId });
    }

    [HttpPost("update/feedback")]
    public async Task<IActionResult> AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      _logger.LogInformation($"{nameof(CommentsController)}.{nameof(AddCommentFeedback)}.Start");
      var result = await _mainDb.Comments.UpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType);
      _logger.LogInformation($"{nameof(CommentsController)}.{nameof(AddCommentFeedback)}.End");
      return result ? new OkResponseResult(new { IsFeedbackUpdated = true }) : new ResponseResult((int)HttpStatusCode.NotModified, new { IsFeedbackUpdated = false });
    }

    [HttpPost]
    public async Task<IActionResult> GetComments([FromBody] GetCommentsViewModel commentsVm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      _logger.LogInformation($"{nameof(CommentsController)}.{nameof(GetComments)}.Start");
      var comments = await _mainDb.Comments.GetComments(commentsVm.UserId, commentsVm.QuestionId, commentsVm.PageParams.Offset, commentsVm.PageParams.Count);
      await _hub.Monitoring.UpdateUrlMonitoring(commentsVm.UserId, UrlMonitoringType.GetsComments);
      _logger.LogInformation($"{nameof(CommentsController)}.{nameof(GetComments)}.End");
      return new OkResponseResult(comments);
    }
  }
}