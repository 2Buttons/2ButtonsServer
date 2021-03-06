﻿using System.Collections.Generic;
using System.Linq;
using AccountData.Main.Queries;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries.MediaFolders;

namespace AccountServer.ViewModels
{
  public static class MappingNotifications
  {
    public static List<NotificationViewModel> MapNotificationDbToViewModel(this IEnumerable<NotificationQuery> notifications, MediaConverter mediaConverter)
    {
      return notifications.Select(n => new NotificationViewModel
          {
            UserId = n.UserId,
            FirstName = n.FirstName,
            LastName = n.LastName,
            SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(n.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
            ActionType = n.ActionType,  /*1 - follow, 2 - recommend, 3 - answer*/
            EmmiterId = n.EmmiterId,
            ActionDate = n.ActionDate
          }
        )
        .ToList();
    }
  }
}
