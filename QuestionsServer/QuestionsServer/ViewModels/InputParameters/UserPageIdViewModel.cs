using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel : UserIdViewModel
  {
    [IdValidation]
    public int UserPageId { get; set; }
  }
}