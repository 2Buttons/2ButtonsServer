using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace AuthorizationServer.ViewModels.InputParameters
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
    public DateTime BirthDate { get; set; } = DateTime.Now.Date;

    [Required]
    public SexType SexType { get; set; }

    public string Phone { get; set; } = null;

    public string Email { get; set; } = null;

    [Required]
    public string City { get; set; }

    public string Description { get; set; } = null;
  }
}