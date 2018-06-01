using System;
using CommonLibraries;

namespace NotificationServer.Models
{
  public class Notification
  {
    public int UserId { get; set; }
    public int SendToId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public ActionType ActionType { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
    public int EmmiterId { get; set; }
    public DateTime ActionDate { get; set; }
  }
}
