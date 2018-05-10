using AccountServer.Helpers;
using AccountServer.Models;
using CommonLibraries;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserContactsViewModel
  {
    public SocialNetType SocialNetType { get; set; }
    public string AccountUrl { get; set; }
  }
}