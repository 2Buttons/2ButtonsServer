using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class LoginRefreshTokenViewModel
  {
    [Required]
    public string RefreshToken { get; set; }
  }
}