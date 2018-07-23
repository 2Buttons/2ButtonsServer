using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class QuestionIdViewModel : UserIdViewModel
  {
    [IdValidation(nameof(QuestionId))]
    public int QuestionId { get; set; }
  }
}