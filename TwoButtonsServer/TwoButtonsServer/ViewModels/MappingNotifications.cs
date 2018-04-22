using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsServer.ViewModels
{
    public static class MappingNotifications
    {
        public static List<NotificationViewModel> MapNotificationDbToViewModel(this IEnumerable<NotificationDb> notifications)
        {
            return notifications.Select(n => new NotificationViewModel
            {
                UserId = n.UserId,
                Login = n.Login,
                SmallAvatarLink = n.SmallAvatarLink,
                Action = n.Action,  /*1 - follow, 2 - recommend, 3 - answer*/
                Desctiption = DescribeAction(n.Action),
                EmmiterId = n.EmmiterId,
                ActionDate = n.ActionDate
                }
            )
                .ToList();
        }

        private static string DescribeAction(int? action)
        {
            switch (action)
            {
                case 1:
                    return "follow";
                case 2:
                    return "recommend";
                case 3:
                    return "answer";
                default:
                    return string.Empty;
            }
        }
    }
}
