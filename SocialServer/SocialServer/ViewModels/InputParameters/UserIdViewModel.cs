using CommonLibraries.Validation;

namespace SocialServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [IdValidation]
    public int UserId { get; set; }
  }
}
