using System;
using CommonLibraries;

namespace NotificationsServer.Models
{
  public class Notification
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public ActionType ActionType { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
    public int EmmiterId { get; set; }
    public DateTime ActionDate { get; set; }
  }

  public class NotificationPair
  {
    public int SendToId { get; set; }

    public Notification Notification { get; set; }
  }
}
