﻿using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;
using NotificationsData;
using NotificationServer.Services;
using NotificationServer.ViewModels.Input;

namespace NotificationServer.Controllers
{
  //[EnableCors("AllowAllOrigin")]
  //[Produces("application/json")]
  [Route("internal")]
  public class InternalNotificationsController : Controller //To get user's posts
  {
    private readonly INotificationsMessageService _notificationsMessageHandler;

    public InternalNotificationsController(INotificationsMessageService notificationsMessageHandler)
    {
      _notificationsMessageHandler = notificationsMessageHandler;
    }

    [HttpPost("follow")]
    public async Task<IActionResult> NotifyFollow([FromBody] FollowNotification notification)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      await _notificationsMessageHandler.PushFolloweNotification(notification);
      return new OkResponseResult();
    }

    [HttpPost("recommendQuestion")]
    public async Task<IActionResult> NotifyRecommendedQuestions([FromBody] RecommendedQuestionNotification notification)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      await _notificationsMessageHandler.PushRecommendedQuestionsNotification(notification);
      return new OkResponseResult();
    }

    [HttpPost("comment")]
    public async Task<IActionResult> NotifyComment([FromBody] CommentNotification notification)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      await _notificationsMessageHandler.CommentNotification(notification);
      return new OkResponseResult();
    }
  }
}