using System.ComponentModel.DataAnnotations;
using AccountServer.Models;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserRegistrationViewModel
  {
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public SexType SexType { get; set; }

    public string City { get; set; } = null;
    [Required]
    public string Phone { get; set; }
    public string Description { get; set; } = null;
    public string FullAvatarLink { get; set; } = null;
    public string SmallAvatarLink { get; set; } = null;
  }
}