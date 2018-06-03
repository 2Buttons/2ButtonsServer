using System.Threading.Tasks;
using CommonLibraries;
using NotificationsData;
using NotificationServer.Models;
using NotificationServer.ViewModels.Input;
using NotificationServer.WebSockets;

namespace NotificationServer.Services
{
  public class NotificationsMessageService : INotificationsMessageService
  {
    private readonly NotificationsDataUnitOfWork _db;
    private readonly WebSocketManager _webSocketManager;

    public NotificationsMessageService(NotificationsDataUnitOfWork db, WebSocketManager webSocketManager)
    {
      _db = db;
      _webSocketManager = webSocketManager;
    }

    public async Task<bool> PushFolloweNotification(FollowNotification followNotification)
    {
      var info = await _db.UsersInfo.FindUserInfoAsync(followNotification.NotifierId);
      if (info == null) return false;
      var notification = new Notification
      {
        UserId = followNotification.NotifierId,
        Login = info.Login,
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = ActionType.Follow,
        EmmiterId = followNotification.NotifierId,
        ActionDate = followNotification.FollowedDate
      };
      PushNotification(followNotification.FollowToId, notification);
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
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = ActionType.Follow,
        EmmiterId = recommendedQuestionNotification.QuestionId,
        ActionDate = recommendedQuestionNotification.RecommendedDate
      };
      PushNotification(recommendedQuestionNotification.UserToId, notification);
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
        SmallAvatarLink = info.SmallAvatarLink,
        ActionType = ActionType.Follow,
        EmmiterId = commentNotification.CommentId,
        ActionDate = commentNotification.CommentedDate
      };
      PushNotification(sendToId, notification);
      return true;
    }

    private void PushNotification(int sendToId, Notification notification)
    {
      _webSocketManager.AddNotification(new NotificationPair {SendToId = sendToId, Notification = notification});
    }
  }
}