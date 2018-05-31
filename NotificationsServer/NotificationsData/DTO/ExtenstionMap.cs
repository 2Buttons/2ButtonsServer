using NotificationsData.Account.Entities;

namespace NotificationsData.DTO
{
  public static class ExtenstionMap
  {
    public static UserDto ToUserDto(this UserDb userDb)
    {
      return new UserDto
      {
        UserId = userDb.UserId,
        AccessFailedCount = userDb.AccessFailedCount,
        Email = userDb.Email,
        EmailConfirmed = userDb.EmailConfirmed,
        PhoneNumber = userDb.PhoneNumber,
        PhoneNumberConfirmed = userDb.PhoneNumberConfirmed,
        RoleType = userDb.RoleType,
        TwoFactorEnabled = userDb.TwoFactorEnabled
      };
    }
  }
}
