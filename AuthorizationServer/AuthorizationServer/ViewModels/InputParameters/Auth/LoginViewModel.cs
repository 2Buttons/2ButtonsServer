using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonTypes;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class LoginViewModel
  {
    [Required]
    public GrantType GrantType { get; set; }
    public ApplicationType ApplicationType { get; set; }

    public string Phone { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }
  }
}