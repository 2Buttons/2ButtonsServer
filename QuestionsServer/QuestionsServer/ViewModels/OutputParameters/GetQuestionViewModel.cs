using System.Collections.Generic;

namespace QuestionsServer.ViewModels.OutputParameters
{
    public class GetQuestionViewModel : QuestionBaseViewModel
  {
    public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
  }
}
