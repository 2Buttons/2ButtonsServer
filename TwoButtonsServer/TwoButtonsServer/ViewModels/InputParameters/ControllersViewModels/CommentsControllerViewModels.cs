using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{

    public class AddCommentViewModel : QuestionIdViewModel
    {
        public string Comment { get; set; }
        public int PreviousCommnetId { get; set; } = 0;

    }

    public class AddCommentFeedbackViewModel : QuestionIdViewModel
    {
        public int Feedback { get; set; }

    }

    public class GetCommentsViewModel : QuestionIdViewModel
    {
        public int Amount { get; set; }

    }
}
