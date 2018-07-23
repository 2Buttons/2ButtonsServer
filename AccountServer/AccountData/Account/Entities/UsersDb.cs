﻿using System;
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
    public bool IsEmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public RoleType RoleType { get; set; }
    public bool IsTwoFactorAuthenticationEnabled { get; set; }
    public bool IsBot { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}
