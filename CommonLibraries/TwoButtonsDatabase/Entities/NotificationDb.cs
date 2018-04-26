using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public class NotificationDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int Action { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
        public int EmmiterId { get; set; }
        public DateTime ActionDate { get; set; }
    }
}