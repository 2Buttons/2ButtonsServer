using System.Threading.Tasks;
using NotificationServer.ViewModels.Input;

namespace NotificationServer.Services
{
  public interface INotificationsMessageService
  {
    Task<bool> CommentNotification(CommentNotification commentNotification);
    Task<bool> PushFolloweNotification(FollowNotification followNotification);
    Task<bool> PushRecommendedQuestionsNotification(RecommendedQuestionNotification recommendedQuestionNotification);
  }
}