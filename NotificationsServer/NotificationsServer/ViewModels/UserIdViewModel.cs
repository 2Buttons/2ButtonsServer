using CommonLibraries.Validation;

namespace NotificationServer.ViewModels
{
  public class UserIdViewModel
  {
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }
  }
}