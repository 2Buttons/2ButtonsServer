using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class UserIdViewModel
  {
    [Required]
    public int UserId { get; set; }
  }
}
