using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public class GetQuestionViewModel : QuestionBaseViewModel
  {
    public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
  }
}
