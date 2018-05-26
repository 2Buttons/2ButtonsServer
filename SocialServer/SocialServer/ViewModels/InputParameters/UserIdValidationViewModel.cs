using CommonLibraries.Validation;

namespace SocialServer.ViewModels.InputParameters
{
  public class UserIdValidationViewModel
  {
    [IdValidation]
    public int UserId { get; set; }
  }
}