using System.Collections.Generic;
using System.Linq;
using CommonLibraries;
using CommonLibraries.MediaFolders;
using NotificationsData.Main.Queries;
using NotificationsServer.Models;

namespace NotificationsServer.ViewModels
{
  public static class MappingNotifications
  {
    public static List<Notification> MapNotificationDbToViewModel(this IEnumerable<NotificationDb> notifications)
    {
      return notifications.Select(n => new Notification
      {
        UserId = n.UserId,
        Login = n.Login,
        SmallAvatarLink = MediaConverter.ToFullAvatarUrl(n.OriginalAvatarUrl, AvatarSizeType.Small),
        ActionType = n.ActionType,  /*1 - follow, 2 - recommend, 3 - answer*/
        EmmiterId = n.EmmiterId,
        ActionDate = n.ActionDate
      }
        )
        .ToList();
    }
  }
}
