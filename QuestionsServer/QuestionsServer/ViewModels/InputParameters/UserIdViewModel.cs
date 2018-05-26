using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [IdValidation]
    public int UserId { get; set; }
  }
}