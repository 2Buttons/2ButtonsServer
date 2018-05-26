using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class QuestionIdViewModel : UserIdViewModel
  {
    [IdValidationt]
    public int QuestionId { get; set; }
  }
}