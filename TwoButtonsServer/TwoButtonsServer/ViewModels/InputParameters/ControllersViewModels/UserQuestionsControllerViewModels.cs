using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class UserQuestionsViewModel: UserPageIdViewModel
    {
        public int QuestionsAmount { get; set; }
        public PhotoParamsViewModel PhotoParams { get; set; } = new PhotoParamsViewModel();
    }

    public class TopUserQuestions : UserIdViewModel
    {
        public DateTime TopAfterDate { get; set; } = DateTime.Now;
        public bool IsOnlyNew { get; set; } = true;
        public int QuestionsAmount { get; set; }
        public PhotoParamsViewModel PhotoParams { get; set; } = new PhotoParamsViewModel();
    }
}
