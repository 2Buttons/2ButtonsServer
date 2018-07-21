using System.Collections.Generic;
using System.Linq;
using AccountData.Main.Queries;
using AccountServer.ViewModels.OutputParameters;

namespace AccountServer.ViewModels
{
  public static class MappingNotifications
  {
    public static List<NotificationViewModel> MapNotificationDbToViewModel(this IEnumerable<NotificationDb> notifications)
    {
      return notifications.Select(n => new NotificationViewModel
          {
            UserId = n.UserId,
            Login = n.Login,
            SmallAvatarUrl = n.SmallAvatarUrl,
            Action = n.Action,  /*1 - follow, 2 - recommend, 3 - answer*/
            EmmiterId = n.EmmiterId,
            ActionDate = n.ActionDate
          }
        )
        .ToList();
    }
  }
}
