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

   
    public async Task<UserDto> FindUserByUserId(int userId)
    {
      var user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x=>x.UserId == userId);
      return user?.ToUserDto();
    }

    public async Task<List<int>> GetUserIdsFromVkIds(IEnumerable<int> vkIds)
    {
      return await _context.SocialsDb.Where(x => x.SocialType == SocialType.Vk)
               .Join(_context.ExternalIdsDb, o => o.ExternalId, i => i.ExternalId, (i, o) => i.ExternalId).ToListAsync() ?? new List<int>();
    }

    public async Task<List<int>> GetUserIdsFromFbIds(IEnumerable<int> fbIds)
    {
        return await _context.SocialsDb.Where(x => x.SocialType == SocialType.Facebook)
          .Join(_context.ExternalIdsDb, o => o.ExternalId, i => i.ExternalId, (i, o) => i.ExternalId).ToListAsync() ?? new List<int>(); 

      /*
        foreach (var fbId in fbIds)
      {
        var userDb = await _context.SocialsDb.Where(x=>x.SocialType == SocialType.Facebook).Join(_context.ExternalIdsDb, x=>x.ExternalId,(i,e)=> new {Id = i}) .Where(x=>x.ExternalId == fbIds..Intersect(.FirstOrDefaultAsync(x => x.SocialType == SocialType.Facebook && x.ExternalId == fbId);
        if (userDb != null)
          result.Add(userDb.InternalId);
      }
      */
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
      return await _context.SocialsDb.Where(x=>x.InternalId == userId).Select(x=> new UserSocialDto{ExternalId = x.ExternalId, SocialType = x.SocialType}).ToListAsync() ?? new List<UserSocialDto>();
    }
  }
}