using CommonLibraries;

namespace AccountServer.ViewModels.InputParameters.Auth
{
  public class LoginViewModel
  {
    public GrantType GrantType { get; set; }
    public ApplicationType ApplicationType { get; set; }

    public string Phone { get; set; }
    public string Password { get; set; }
  }
}