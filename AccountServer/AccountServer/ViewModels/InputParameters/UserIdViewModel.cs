using CommonLibraries.Validation;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [NotDefaultInt(nameof(UserId))]
    public int UserId { get; set; }
  }
}
