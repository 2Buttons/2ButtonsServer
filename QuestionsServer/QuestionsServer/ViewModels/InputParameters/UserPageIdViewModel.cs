using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel : UserIdViewModel
  {
    [IdValidationt]
    public int UserPageId { get; set; }
  }
}