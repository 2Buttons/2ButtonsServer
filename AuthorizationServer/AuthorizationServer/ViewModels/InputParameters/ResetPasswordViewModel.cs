using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class ResetPasswordViewModel
  {
    [Required]
    public string Token { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  }
}