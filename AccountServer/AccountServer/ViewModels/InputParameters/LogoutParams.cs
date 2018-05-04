using System.ComponentModel.DataAnnotations;

namespace AccountServer.ViewModels.InputParameters
{
  public class LogoutParams
  {
    [Required]
    public int ClientId { get; set; }

    [Required]
    public string SecretKey { get; set; }
  }
}