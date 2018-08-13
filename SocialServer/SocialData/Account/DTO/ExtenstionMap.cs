using CommonLibraries.Entities;
using CommonLibraries.Entities.Acccount;
using SocialData.Account.Entities;

namespace SocialData.Account.DTO
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
        TwoFactorEnabled = userDb.IsTwoFactorAuthenticationEnabled,
      };
    }
  }
}
