using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class GetPostsViewModel: UserPageIdViewModel
    {
        public int PostsAmount { get; set; }
    }
}
