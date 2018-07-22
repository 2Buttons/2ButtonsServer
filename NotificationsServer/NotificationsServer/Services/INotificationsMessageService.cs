using System.Threading.Tasks;
using NotificationsServer.ViewModels.Input;

namespace NotificationsServer.Services
{
  public interface INotificationsMessageService
  {
    Task<bool> CommentNotification(CommentNotification commentNotification);
    Task<bool> PushFollowedNotification(FollowNotification followNotification);
    Task<bool> PushRecommendedQuestionsNotification(RecommendedQuestionNotification recommendedQuestionNotification);
  }
}