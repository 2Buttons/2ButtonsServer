using AccountServer.Models;

namespace AccountServer.ViewModels.InputParameters
{
  public class LoginViewModel
  {
    public string GrantType { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public int ClientId { get; set; }
    public string SecretKey { get; set; }
    public ApplicationTypes ApplicationType { get; set; }
  }
}