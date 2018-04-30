using System.ComponentModel.DataAnnotations;

namespace AccountServer.ViewModels.InputParameters
{
    public class UserRegistrationViewModel
    {
      [Required]
    public string Login { get; set; }
      [Required]
      public string Password { get; set; }
    //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
    //public string ConfirmPassword { get; set; }
      [Required]
    public int Age { get; set; }
      [Required]
    public SexType SexType { get; set; }
      public string Phone { get; set; } = null;
      public string Description { get; set; } = null;
      public string FullAvatarLink { get; set; } = null;
      public string SmallAvatarLink { get; set; } = null;
  }

  public enum SexType
  {
    Man = 1,
    Woman =2
  }
}
