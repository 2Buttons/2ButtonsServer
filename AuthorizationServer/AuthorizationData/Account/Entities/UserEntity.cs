using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AuthorizationData.Account.Entities
{
  [Table("Users")]
  public class UserEntity
  {
    [Key]
    public int UserId { get; set; }
    public int AccessFailedCount { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public RoleType RoleType { get; set; }
    public bool IsTwoFactorAuthenticationEnabled { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}
