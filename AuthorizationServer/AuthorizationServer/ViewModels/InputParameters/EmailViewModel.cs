using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.ViewModels.InputParameters
{
  public class EmailViewModel
  {
    [Required]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "The Email field is not a valid e-mail address.")]
    public string Email { get; set; }
  }
}