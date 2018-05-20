using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AccountData.Account.Entities
{
  [Table("Users")]
  public partial class UserDb
  {
    [Key]
    public int UserId { get; set; }
    public int AccessFailedCount { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public RoleType RoleType { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public int VkId { get; set; }
    public string VkToken { get; set; }
    public long FacebookId { get; set; }
    public string FacebookToken { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}
