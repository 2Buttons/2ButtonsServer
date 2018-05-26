using CommonLibraries.Validation;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class UserIdValidationViewModel
  {
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }
  }
}