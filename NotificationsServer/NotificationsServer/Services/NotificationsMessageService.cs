using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.MediaFolders;
using Microsoft.Extensions.Logging;
using NotificationsData;
using NotificationsServer.Models;
using NotificationsServer.ViewModels.Input;
using NotificationsServer.WebSockets;

namespace NotificationsServer.Services
{
  public class NotificationsMessageService : INotificationsMessageService
  {
    private  NotificationsDataUnitOfWork Db { get; }
    private  ConnectionsHub Hub { get; }
    private  NotificationManager NotificationManager { get; }
    private MediaConverter MediaConverter { get; }
    private  ILogger<NotificationsMessageService> Logger { get; }

    public NotificationsMessageService(ConnectionsHub hub, NotificationsDataUnitOfWork db, NotificationManager notificationManager, ILogger<NotificationsMessageService> logger, MediaConverter mediaConverter)
    {
      Db = db;
      Hub = hub;
      NotificationManager = notificationManager;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    public async Task<bool> PushFollowedNotification(FollowNotification followNotification)
    {
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushFollowedNotification)}.Start");
      var info = await Db.UsersInfo.FindUserInfoAsync(followNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = followNotification.NotifierId,
        Login = info.FirstName + " " + info.LastName,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = followNotification.NotifierId,
        ActionDate = followNotification.FollowedDate
      };
      await PushNotification(followNotification.FollowingId, notification);
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushFollowedNotification)}.End");
      return true;
    }

    public async Task<bool> PushRecommendedQuestionsNotification(
      RecommendedQuestionNotification recommendedQuestionNotification)
    {
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushRecommendedQuestionsNotification)}.Start");
      var info = await Db.UsersInfo.FindUserInfoAsync(recommendedQuestionNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = recommendedQuestionNotification.NotifierId,
        Login = info.FirstName + " " + info.LastName,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = recommendedQuestionNotification.QuestionId,
        ActionDate = recommendedQuestionNotification.RecommendedDate
      };
      await PushNotification(recommendedQuestionNotification.UserToId, notification);
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushRecommendedQuestionsNotification)}.End");
      return true;
    }

    public async Task<bool> CommentNotification(CommentNotification commentNotification)
    {
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(CommentNotification)}.Start");
      var info = await Db.UsersInfo.FindUserInfoAsync(commentNotification.NotifierId);
      if (info == null) return false;
      var sendToId = await Db.Notifications.GetUserIdForComment(commentNotification.CommentId);
      if (sendToId <= 0) return false;

      var notification = new Notification
      {
        UserId = commentNotification.NotifierId,
        Login = info.FirstName + " " + info.LastName,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(info.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = ActionType.Follow,
        EmmiterId = commentNotification.CommentId,
        ActionDate = commentNotification.CommentedDate
      };
      await PushNotification(sendToId, notification);
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(CommentNotification)}.End");
      return true;
    }

    private async Task PushNotification(int sendToId, Notification notification)
    {
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushNotification)}.Start SendToId: {sendToId}");
      await NotificationManager.AddNotification(new NotificationPair {SendToId = sendToId, Notification = notification});
      await Hub.Monitoring.UpdateUrlMonitoring(sendToId, UrlMonitoringType.GetsNotifications);
      Logger.LogInformation($"{nameof(NotificationsMessageService)}.{nameof(PushNotification)}.End");
    }
  }
}