using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [IdValidationt]
    public int UserId { get; set; }
  }
}