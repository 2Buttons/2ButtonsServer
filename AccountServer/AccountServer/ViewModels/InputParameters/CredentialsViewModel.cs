using AccountServer.Models;
using CommonLibraries;

namespace AccountServer.ViewModels.InputParameters
{
  public class CredentialsViewModel
  {
    public GrantType GrantType { get; set; }
    public ApplicationType ApplicationType { get; set; }

    public string Phone { get; set; }
    public string Password { get; set; }

    public string RefreshToken { get; set; }
    public int ClientId { get; set; }
    public string SecretKey { get; set; }


    public bool IsRememberMe { get; set; }
  }
}