using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
  public class GetFollowToViewModel :GetFollowerViewModel
  {
    public int VisitsAmount { get; set; }
  }
}
