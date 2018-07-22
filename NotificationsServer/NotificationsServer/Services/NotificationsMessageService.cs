﻿using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.MediaFolders;
using NotificationsData;
using NotificationsServer.Models;
using NotificationsServer.ViewModels.Input;
using NotificationsServer.WebSockets;

namespace NotificationsServer.Services
{
  public class NotificationsMessageService : INotificationsMessageService
  {
    private readonly NotificationsDataUnitOfWork _db;
    private readonly ConnectionsHub _hub;
    private readonly NotificationManager _notificationManager;
   // private readonly WebSocketManager _webSocketManager;

    public NotificationsMessageService(ConnectionsHub hub, NotificationsDataUnitOfWork db, NotificationManager notificationManager)
    {
      _db = db;
      _hub = hub;
      _notificationManager = notificationManager;
      // _webSocketManager = webSocketManager;
    }

    public async Task<bool> PushFollowedNotification(FollowNotification followNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(followNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = followNotification.NotifierId,
        Login = info.Login,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = followNotification.NotifierId,
        ActionDate = followNotification.FollowedDate
      };
      await PushNotification(followNotification.FollowingId, notification);
      return true;
    }

    public async Task<bool> PushRecommendedQuestionsNotification(
      RecommendedQuestionNotification recommendedQuestionNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(recommendedQuestionNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = recommendedQuestionNotification.NotifierId,
        Login = info.Login,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = recommendedQuestionNotification.QuestionId,
        ActionDate = recommendedQuestionNotification.RecommendedDate
      };
      await PushNotification(recommendedQuestionNotification.UserToId, notification);
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
        Login = info.Login,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = commentNotification.CommentId,
        ActionDate = commentNotification.CommentedDate
      };
      await PushNotification(sendToId, notification);
      return true;
    }

    private async Task PushNotification(int sendToId, Notification notification)
    {
      await _notificationManager.AddNotification(new NotificationPair {SendToId = sendToId, Notification = notification});
      await _hub.Monitoring.UpdateUrlMonitoring(sendToId, UrlMonitoringType.GetsNotifications);
    }
  }
}