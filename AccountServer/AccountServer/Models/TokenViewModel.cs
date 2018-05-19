using CommonLibraries;

namespace AccountServer.Models
{
  public class Token
  {
    public int UserId { get; set; }
    public RoleType RoleType { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long ExpiresIn { get; set; }
  }
}