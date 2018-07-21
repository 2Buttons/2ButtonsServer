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

    [Compare("Password", ErrorMessage = "Passwords are not similar.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public DateTime BirthDate { get; set; } = DateTime.Now.Date;

    [Required]
    public SexType SexType { get; set; }

    public string Phone { get; set; } = null;
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "The Email field is not a valid e-mail address.")]
    public string Email { get; set; } = null;

    [Required]
    public string City { get; set; }

    public string Description { get; set; } = null;
  }
}