using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public class GetFollowerViewModel
    {
      public int UserId { get; set; }
      public string Login { get; set; }
      public string SmallAvatarLink { get; set; }
      public int Age { get; set; }
      public int Sex { get; set; }
      public bool IsYouFollowed { get; set; }
      public bool IsHeFollowed { get; set; }
  }
}
