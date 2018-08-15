using CommonLibraries.Entities.Acccount;

namespace AccountData.DTO
{
  public static class ExtenstionMap
  {
    public static UserDto ToUserDto(this UserEntity userDb)
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
        TwoFactorEnabled = userDb.IsTwoFactorAuthenticationEnabled
      };
    }
  }
}
