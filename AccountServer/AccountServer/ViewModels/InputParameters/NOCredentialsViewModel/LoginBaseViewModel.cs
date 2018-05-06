using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Entities;

namespace AccountServer.ViewModels.InputParameters.NOCredentialsViewModel
{
  public class LoginBaseViewModel
  {
    public GrantType GrantType { get; set; }
    public int ClientId { get; set; }
    public string SecretKey { get; set; }
    public ApplicationType ApplicationType { get; set; }
  }
}
