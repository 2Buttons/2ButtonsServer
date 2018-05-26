



using CommonLibraries.Validation;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class UserIdValidationViewModel
  {

    [NotDefaultIntAttribute(nameof(UserId))]
    public int UserId { get; set; }
  }
}