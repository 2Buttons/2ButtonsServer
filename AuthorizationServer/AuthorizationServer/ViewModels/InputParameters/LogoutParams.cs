using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class LogoutParams
  {
    [Required]
    public string RefreshToken { get; set; }
  }
}