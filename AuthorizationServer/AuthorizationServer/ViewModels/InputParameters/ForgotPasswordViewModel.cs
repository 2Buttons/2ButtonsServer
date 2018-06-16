using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class ForgotPasswordViewModel
  {
    [Required]
    public string Email { get; set; }
  }
}