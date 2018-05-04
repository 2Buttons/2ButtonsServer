using AccountServer.Models;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserContactsViewModel
  {
    public SocialNetType NetworkType { get; set; }
    public string ContactsAccount { get; set; }
  }
}