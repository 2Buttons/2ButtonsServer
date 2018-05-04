using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.ViewModels.OutputParameters
{
  public class IdentityRespose
  {
    public string AccessToken { get; set; }
    public int UserId { get; set; }
    public int ClientId { get; set; }
    public string ClientSecret { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
  }
}
