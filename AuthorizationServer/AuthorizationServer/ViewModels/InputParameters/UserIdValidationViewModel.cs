using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class UserIdValidationViewModel
  {
    [Required]
    public int UserId { get; set; }
  }
}