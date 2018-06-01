using CommonLibraries;

namespace NotificationsData.DTO
{
    public class UserDto
    {
      public int UserId { get; set; }
      public int AccessFailedCount { get; set; }
      public string Email { get; set; }
      public bool EmailConfirmed { get; set; }
      public string PhoneNumber { get; set; }
      public bool PhoneNumberConfirmed { get; set; }
      public RoleType RoleType { get; set; }
      public bool TwoFactorEnabled { get; set; }
  }
}
