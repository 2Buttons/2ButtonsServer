using AuthorizationServer.Models;
using AuthorizationServer.ViewModels.OutputParameters.User;

namespace AuthorizationServer.ViewModels.OutputParameters
{
  public class LoginPairViewModel
  {
    public Token Token { get; set; }
    public UserInfoViewModel User { get; set; }
  }
}