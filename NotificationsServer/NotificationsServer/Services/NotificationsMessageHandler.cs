using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using NotificationsData;
using NotificationServer.Models;
using NotificationServer.ViewModels.Input;

namespace NotificationServer.Services
{
  public class NotificationsMessageHandler
  {

    private readonly NotificationsDataUnitOfWork _db;
    private readonly ConcurrentQueue<Notification> _notifications = new ConcurrentQueue<Notification>();

    public NotificationsMessageHandler(NotificationsDataUnitOfWork db)
    {
      _db = db;
    }

    public async Task<bool> PushFolloweNotification(FollowNotification followNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(followNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = followNotification.NotifierId,
        SendToId = followNotification.FollowToId,
        Login = info.Login,
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = CommonLibraries.ActionType.Follow,
        EmmiterId = followNotification.NotifierId,
        ActionDate = followNotification.FollowedDate
      };
      PushNotification(notification);
      return true;
    }

    public async Task<bool> PushRecommendedQuestionsNotification(RecommendedQuestionNotification recommendedQuestionNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(recommendedQuestionNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = recommendedQuestionNotification.NotifierId,
        SendToId = recommendedQuestionNotification.UserToId,
        Login = info.Login,
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = CommonLibraries.ActionType.Follow,
        EmmiterId = recommendedQuestionNotification.QuestionId,
        ActionDate = recommendedQuestionNotification.RecommendedDate
      };
      PushNotification(notification);
      return true;
    }

    public async Task<bool> CommentNotification(CommentNotification commentNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(commentNotification.NotifierId);
      if (info == null) return false;
      var sendToId = await _db.Notifications.GetUserIdForComment(commentNotification.CommentId);
      if (sendToId <= 0) return false;

      var notification = new Notification
      {
        UserId = commentNotification.NotifierId,
        SendToId = sendToId,
        Login = info.Login,
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = CommonLibraries.ActionType.Follow,
        EmmiterId = commentNotification.CommentId,
        ActionDate = commentNotification.CommentedDate
      };
      PushNotification(notification);
      return true;
    }

    private void PushNotification(Notification notification)
    {
      _notifications.Enqueue(notification);
    }
  }
}
