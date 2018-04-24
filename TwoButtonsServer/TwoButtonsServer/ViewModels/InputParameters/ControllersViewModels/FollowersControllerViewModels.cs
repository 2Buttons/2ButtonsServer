using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class FollowerViewModel : UserPageIdViewModel
    {
        public int Amount { get; set; } = 100;
        public string SearchedLogin { get; set; } = "";
    }
}
