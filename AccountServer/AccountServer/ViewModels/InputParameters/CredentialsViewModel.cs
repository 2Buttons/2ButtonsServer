using AccountServer.Entities;
using AccountServer.Models;

namespace AccountServer.ViewModels.InputParameters
{
  public class LoginViewModel
  {
    public GrantType GrantType { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public int ClientId { get; set; }
    public string SecretKey { get; set; }
    public ApplicationType ApplicationType { get; set; }
  }
}