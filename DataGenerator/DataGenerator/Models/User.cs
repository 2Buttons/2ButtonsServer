using System;
using CommonLibraries;

namespace DataGenerator.Models
{
  public class User
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string OriginalAvatarUrl { get; set; }

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