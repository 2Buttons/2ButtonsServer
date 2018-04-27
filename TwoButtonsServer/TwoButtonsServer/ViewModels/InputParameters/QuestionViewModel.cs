using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class QuestionViewModel : QuestionIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
    }
}
