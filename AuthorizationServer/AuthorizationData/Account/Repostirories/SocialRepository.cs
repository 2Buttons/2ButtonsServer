using System;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account.Repostirories
{
  public class SocialRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _contextAccount;

    public SocialRepository(TwoButtonsAccountContext context)
    {
      _contextAccount = context;
    }

    public void Dispose()
    {
      _contextAccount.Dispose();
    }

    public async Task<UserDto> FindUserByExternalUserIdAsync(long externalId, SocialType socialType)
    {
      return await _contextAccount.SocialsDb.Where(x => x.SocialType == socialType && x.ExternalId == externalId).Join(_contextAccount.UsersDb, i => i.InternalId, o => o.UserId, (i, o) => new UserDto
      {
        UserId = o.UserId,
        AccessFailedCount = o.AccessFailedCount,
        Email = o.Email,
        EmailConfirmed = o.EmailConfirmed,
        PhoneNumber = o.PhoneNumber,
        PhoneNumberConfirmed = o.PhoneNumberConfirmed,
        RoleType = o.RoleType,
        TwoFactorEnabled = o.TwoFactorEnabled
      }).FirstOrDefaultAsync();
    }

    public async Task<UserDto> FindUserByExternalEmaildAsync(string externalEmail)
    {
      return await _contextAccount.SocialsDb.Where(x => x.Email == externalEmail).Join(_contextAccount.UsersDb, i => i.InternalId, o => o.UserId, (i, o) => new UserDto
      {
        UserId = o.UserId,
        AccessFailedCount = o.AccessFailedCount,
        Email = o.Email,
        EmailConfirmed = o.EmailConfirmed,
        PhoneNumber = o.PhoneNumber,
        PhoneNumberConfirmed = o.PhoneNumberConfirmed,
        RoleType = o.RoleType,
        TwoFactorEnabled = o.TwoFactorEnabled
      }).FirstOrDefaultAsync();
    }

    public async Task<bool> AddUserSocialAsync(SocialDb social)
    {
      _contextAccount.SocialsDb.Add(social);
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    //public async Task<long> GetExternalUserIdAsync(int userId, SocialType socialType)
    //{
    //  var user = await _contextAccount.UsersDb.FindAsync(userId);
    //  if (user == null) return 0;
    //  switch (socialType)
    //  {
    //    case SocialType.Facebook:
    //      return user.FacebookId;
    //    case SocialType.Vk:
    //      return user.VkId;
    //    case SocialType.Twiter:
    //    case SocialType.GooglePlus:
    //    case SocialType.Telegram:
    //    case SocialType.Badoo:
    //    case SocialType.Nothing:
    //    default:
    //      return 0;
    //  }
    //}
  }
}