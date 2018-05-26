using CommonLibraries.Validation;

namespace SocialServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel : UserIdViewModel
  {
    [IdValidation]
    public int UserPageId { get; set; }
  }
}
