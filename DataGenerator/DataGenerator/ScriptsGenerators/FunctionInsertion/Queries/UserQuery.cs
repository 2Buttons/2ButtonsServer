using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class UserQuery
  {
    public int UserId { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public RoleType RoleType { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool IsBot { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}