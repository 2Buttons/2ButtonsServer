using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("questions/comments")]
  public class CommentsController : Controller
  {
    private QuestionsUnitOfWork MainDb { get; }
    private ConnectionsHub Hub { get; }
    private ILogger<ComplaintsController> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public CommentsController(QuestionsUnitOfWork mainDb, ConnectionsHub hub, ILogger<ComplaintsController> logger,
      MediaConverter mediaConverter)
    {
      MainDb = mainDb;
      Hub = hub;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel comment)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (comment.PreviousCommentId == 0) comment.PreviousCommentId = null;
      var commentId = await MainDb.Comments.AddComment(comment.UserId, comment.QuestionId, comment.Text,
        comment.PreviousCommentId);
      if (commentId < 0)
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not create comment");
      if (comment.PreviousCommentId > 0)
        await Hub.Notifications.SendCommentNotification(comment.UserId, comment.QuestionId, commentId, DateTime.UtcNow);
      return new ResponseResult((int) HttpStatusCode.Created, new {CommentId = commentId});
    }

    [HttpPost("update/feedback")]
    public async Task<IActionResult> AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(CommentsController)}.{nameof(AddCommentFeedback)}.Start");
      var result = await MainDb.Comments.UpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType);
      Logger.LogInformation($"{nameof(CommentsController)}.{nameof(AddCommentFeedback)}.End");
      return result
        ? new OkResponseResult(new {IsFeedbackUpdated = true})
        : new ResponseResult((int) HttpStatusCode.NotModified, new {IsFeedbackUpdated = false});
    }

    [HttpPost]
    public async Task<IActionResult> GetComments([FromBody] GetCommentsViewModel commentsVm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(CommentsController)}.{nameof(GetComments)}.Start");
      var comments = await MainDb.Comments.GetComments(commentsVm.UserId, commentsVm.QuestionId,
        commentsVm.PageParams.Offset, commentsVm.PageParams.Count);
      await Hub.Monitoring.UpdateUrlMonitoring(commentsVm.UserId, UrlMonitoringType.GetsComments);
      Logger.LogInformation($"{nameof(CommentsController)}.{nameof(GetComments)}.End");
      return new OkResponseResult(comments.Select(x => new CommentViewModel
      {
        CommentId = x.CommentId,
        DislikesCount = x.DislikesCount,
        LikesCount = x.LikesCount,
        Login = x.Login,
        PreviousCommentId = x.PreviousCommentId,
        Text = x.Text,
        UserId = x.UserId,
        YourFeedbackType = x.YourFeedbackType,
        CommentedDate = x.CommentedDate,
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(x.OriginalAvatarUrl, AvatarSizeType.Small)
      }).ToList());
    }
  }
}