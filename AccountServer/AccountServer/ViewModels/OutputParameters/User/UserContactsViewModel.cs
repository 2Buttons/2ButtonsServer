using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserContactsViewModel
  {
    public SocialNetType NetworkType { get; set; }
    public string ContactsAccount { get; set; }
  }
}
