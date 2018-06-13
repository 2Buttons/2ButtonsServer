using CommonLibraries.Validation;

namespace NotificationsServer.ViewModels
{
  public class UserIdViewModel
  {
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }
  }
}