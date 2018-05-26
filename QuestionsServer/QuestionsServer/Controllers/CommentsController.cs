﻿using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  //[Route("api/[controller]")]
  public class CommentsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public CommentsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("addComment")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel comment)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var comentId = await _mainDb.Comments.AddComment(comment.UserId, comment.QuestionId, comment.CommentText,
        comment.PreviousCommnetId);
      return new ResponseResult((int)HttpStatusCode.Created, new { CommentId = comentId });
    }

    [HttpPost("updateFeedback")]
    public async Task<IActionResult> AddCommentFeedback([FromBody] AddCommentFeedbackViewModel commentFeedback)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (await _mainDb.Comments.UpdateCommentFeedback(commentFeedback.UserId, commentFeedback.CommentId,
        commentFeedback.FeedbackType)) return new OkResponseResult(new { IsFeedbackUpdated = true });
      return new ResponseResult((int)HttpStatusCode.NotModified, new { IsFeedbackUpdated = false });
    }

    [HttpPost("getComments")]
    public async Task<IActionResult> GetComments([FromBody] GetCommentsViewModel commentsVm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      var comments = await _mainDb.Comments.GetComments(commentsVm.UserId, commentsVm.QuestionId, commentsVm.Amount);
      return new OkResponseResult(comments);
    }
  }
}