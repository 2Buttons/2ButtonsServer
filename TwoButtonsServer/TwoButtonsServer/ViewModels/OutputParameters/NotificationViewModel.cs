using System;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public class NotificationViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public ActionType Action { get; set; } /*1 - follow, 2 - recommend, 3 - answer*/
        public int EmmiterId { get; set; }
        public DateTime ActionDate { get; set; } 
    }

  public enum ActionType
  {
    Follow=1,
    Recommend = 2,
    Answer = 3
  }

}
