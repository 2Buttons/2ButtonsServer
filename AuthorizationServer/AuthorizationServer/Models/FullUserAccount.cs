using AuthorizationData.Account.DTO;
using AuthorizationData.Main.Entities;

namespace AuthorizationServer.Models
{
  public class FullUserAccount
  {
    public UserDto UserAuth { get; set; }
    public UserMainDb UserMain { get; set; }
  }
}