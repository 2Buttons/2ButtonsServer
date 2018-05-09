using AccountServer.Helpers;
using AccountServer.Models;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserContactsViewModel
  {
    public SocialNetType SocialNetType { get; set; }
    public string AccountUrl { get; set; }
  }
}