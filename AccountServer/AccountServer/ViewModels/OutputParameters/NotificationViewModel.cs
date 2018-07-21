using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace AccountServer.ViewModels.OutputParameters
{
  public class NotificationViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarUrl { get; set; }
    public ActionType Action { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
    public int EmmiterId { get; set; }
    public DateTime ActionDate { get; set; }
  }
}
