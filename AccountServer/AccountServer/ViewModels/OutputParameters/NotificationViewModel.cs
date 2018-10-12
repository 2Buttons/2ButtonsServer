using System;
using CommonLibraries;

namespace AccountServer.ViewModels.OutputParameters
{
  public class NotificationViewModel
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SmallAvatarUrl { get; set; }
    public ActionType ActionType { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
    public int EmmiterId { get; set; }
    public DateTime ActionDate { get; set; }
  }
}
