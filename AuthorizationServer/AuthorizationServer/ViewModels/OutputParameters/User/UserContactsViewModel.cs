using CommonLibraries;
using CommonTypes;

namespace AuthorizationServer.ViewModels.OutputParameters.User
{
  public class UserContactsViewModel
  {
    public SocialType SocialType { get; set; }
    public string AccountUrl { get; set; }
  }
}