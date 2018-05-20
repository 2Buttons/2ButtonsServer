using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;
using TwoButtonsAccountDatabase.Entities;

namespace TwoButtonsAccountDatabase.DTO
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
        TwoFactorEnabled = userDb.TwoFactorEnabled,
        VkId = userDb.VkId,
        VkToken = userDb.VkToken,
        FacebookId = userDb.FacebookId,
        FacebookToken = userDb.FacebookToken
      };
    }
  }
}
