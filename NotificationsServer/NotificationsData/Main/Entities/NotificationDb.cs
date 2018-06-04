using CommonLibraries;
using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationsData.Main.Entities
{
    public class NotificationDb
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public ActionType Action { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
        public int EmmiterId { get; set; }
        public DateTime ActionDate { get; set; }
    }
}