using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class RefreshViewModel
  {
    [Required]
    public string RefreshToken { get; set; }
  }
}