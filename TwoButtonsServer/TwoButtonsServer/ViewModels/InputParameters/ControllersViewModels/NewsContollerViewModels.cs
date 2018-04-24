using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class GetNewsViewModel : UserIdViewModel
    {
        public int QuestionsAmount { get; set; }
        public PhotoParamsViewModel PhotoParams { get; set; } = new PhotoParamsViewModel();
    }
}
