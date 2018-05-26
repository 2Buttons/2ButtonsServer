using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class QuestionIdViewModel : UserIdViewModel
  {
    [IdValidation]
    public int QuestionId { get; set; }
  }
}