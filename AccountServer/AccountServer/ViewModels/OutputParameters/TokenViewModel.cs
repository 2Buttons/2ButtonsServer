using CommonLibraries;

namespace AccountServer.ViewModels.OutputParameters
{
  public class TokenViewModel
  {
    public int UserId { get; set; }
    public RoleType RoleType { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long ExpiresIn { get; set; }
  }
}