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
      return await _contextAccount.SocialEntities.Where(x => x.SocialType == socialType && x.ExternalId == externalId).Join(_contextAccount.UserEntities, i => i.InternalId, o => o.UserId, (i, o) => new UserDto
      {
        UserId = o.UserId,
        AccessFailedCount = o.AccessFailedCount,
        Email = o.Email,
        EmailConfirmed = o.IsEmailConfirmed,
        PhoneNumber = o.PhoneNumber,
        PhoneNumberConfirmed = o.IsPhoneNumberConfirmed,
        RoleType = o.RoleType,
        TwoFactorEnabled = o.IsTwoFactorAuthenticationEnabled
      }).LastOrDefaultAsync();
    }

    public async Task<bool> UpdateExternalAccessToken(long externalId, SocialType socialType, string externalToken, long expiresIn)
    {
      var social = await 
        _contextAccount.SocialEntities.FirstOrDefaultAsync(x => x.SocialType == socialType && x.ExternalId == externalId);
      social.ExternalToken = externalToken;
      social.ExpiresIn = expiresIn;
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<UserDto> FindUserByExternalEmaildAsync(string externalEmail)
    {
      return await _contextAccount.SocialEntities.Where(x => x.Email == externalEmail).Join(_contextAccount.UserEntities, i => i.InternalId, o => o.UserId, (i, o) => new UserDto
      {
        UserId = o.UserId,
        AccessFailedCount = o.AccessFailedCount,
        Email = o.Email,
        EmailConfirmed = o.IsEmailConfirmed,
        PhoneNumber = o.PhoneNumber,
        PhoneNumberConfirmed = o.IsPhoneNumberConfirmed,
        RoleType = o.RoleType,
        TwoFactorEnabled = o.IsTwoFactorAuthenticationEnabled
      }).FirstOrDefaultAsync();
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
      return await _contextAccount.SocialEntities.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task<bool> AddUserSocialAsync(SocialEntity social)
    {
      _contextAccount.SocialEntities.Add(social);
      return await _contextAccount.SaveChangesAsync() > 0;
    }
  }
}