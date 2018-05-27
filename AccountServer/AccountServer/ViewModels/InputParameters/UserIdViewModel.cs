using CommonLibraries.Validation;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }
  }
}
