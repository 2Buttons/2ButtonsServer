using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using SocialData.Account.DTO;
using SocialData.Account.Entities;
using SocialData.Account.Entities.FunctionEntities;

namespace SocialData.Account.Repostirories
{
  public class SocialRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;

    public SocialRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }

    public void Dispose()
    {
      _context.Dispose();
    }

   
    public async Task<UserDto> GetUserByUserId(int userId)
    {
      var user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x=>x.UserId == userId);
      return user?.ToUserDto();

    }

    public async Task<List<int>> GetUserIdsFromVkIds(IEnumerable<int> vkIds)
    {
      var result = new List<int>();

      foreach (var vkId in vkIds)
      {
        var userDb = await _context.SocialsDb.FirstOrDefaultAsync(x => x.SocialType == SocialType.Vk && x.ExternalId == vkId);
        if (userDb != null)
          result.Add(userDb.InternalId);
      }
      return result;
    }

    public async Task<List<int>> GetUserIdsFromFbIds(IEnumerable<int> fbIds)
    {
      var result = new List<int>();

      foreach (var fbId in fbIds)
      {
        var userDb = await _context.SocialsDb.FirstOrDefaultAsync(x => x.SocialType == SocialType.Facebook && x.ExternalId == fbId);
        if (userDb != null)
          result.Add(userDb.InternalId);
      }
      return result;
    }

    //public async Task<int> GetUserIdByExternalUserIdAsync(int externalUserId, SocialType socialType)
    //{
    //  return (await _context.SocialsDb.FirstOrDefaultAsync(x => x.ExternalId == externalUserId && x.SocialType == socialType))?.InternalId ?? 0;
    //}

    //public async Task<long> GetExternalUserIdAsync(int userId, SocialType socialType)
    //{
    //  return (await _context.SocialsDb.FirstOrDefaultAsync(x => x.InternalId == userId && x.SocialType == socialType))
    //    ?.ExternalId ?? 0;
    //}

    //public async Task<bool> IsUserIdExistAsync(int userId)
    //{
    //  return await _context.UsersDb.FindAsync(userId) != null;
    //}

    //public async Task<bool> IsUserExistByPhoneAsync(string phone)
    //{
    //  return await _context.UsersDb.AsNoTracking().AnyAsync(x => x.PhoneNumber == phone);
    //}

    //public async Task<bool> IsUserExistByEmailAsync(string email)
    //{
    //  return await _context.UsersDb.AsNoTracking().AnyAsync(x=>x.Email == email);
    //}

    public async Task<List<UserSocialDto>> GetUserSocialsAsync(int userId)
    {
      var socials = await _context.SocialsDb.Where(x=>x.InternalId == userId).ToListAsync();
      return socials?.Select(x => new UserSocialDto {SocialType = x.SocialType, ExternalId = x.ExternalId}).ToList() ?? new List<UserSocialDto>();
    }
  }
}