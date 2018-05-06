namespace AccountServer.ViewModels.OutputParameters
{
  public class TokenViewModel
  {
    public string AccessToken { get; set; }
    public int UserId { get; set; }
    public int ClientId { get; set; }
    public string SecretKey { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
  }
}