using AuthorizationData.Account.Entities;

namespace AuthorizationData.Account.DTO
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
        EmailConfirmed = userDb.IsEmailConfirmed,
        PhoneNumber = userDb.PhoneNumber,
        PhoneNumberConfirmed = userDb.IsPhoneNumberConfirmed,
        RoleType = userDb.RoleType,
        TwoFactorEnabled = userDb.IsTwoFactorAuthenticationEnabled,
      };
    }
  }
}
