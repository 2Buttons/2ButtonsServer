using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsData.Main.Entities;
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
            SmallAvatarLink = n.SmallAvatarLink,
            Action = n.Action,  /*1 - follow, 2 - recommend, 3 - answer*/
            EmmiterId = n.EmmiterId,
            ActionDate = n.ActionDate
          }
        )
        .ToList();
    }
  }
}
