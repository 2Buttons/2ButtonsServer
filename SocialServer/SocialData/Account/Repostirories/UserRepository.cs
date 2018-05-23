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
  public class UserRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;

    public UserRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    public async Task<bool> AddOrChangeExternalUserIdAsync(int userId, int externalUserId, SocialNetType socialType,
      string externalToken)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return false;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user.FacebookId = externalUserId;
          user.FacebookToken = externalToken;
          break;
        case SocialNetType.Vk:
          user.VkId = externalUserId;
          user.VkToken = externalToken;
          break;
        case SocialNetType.Nothing:
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        default:
          return false;
      }

      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
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
        var userDb = await _context.UsersDb.FirstOrDefaultAsync(x => x.VkId == vkId);
        if (userDb != null)
          result.Add(userDb.UserId);
      }
      return result;
      //var dataTable = new DataTable();
      //dataTable.Columns.Add("id", typeof(int));
      //foreach (var id in ids)
      //{
      //  dataTable.Rows.Add(id);
      //}
      //var vkIdTable = new SqlParameter
      //{
      //  ParameterName = "@VkIdTable",
      //  TypeName="dbo.idTable",
      //  Value = dataTable
      //};
      //return _context.UserIds.FromSql($"select * from dbo.[getRecommendedFromUsersID](@VkIdTable)", vkIdTable)
      //  .ToList();
    }

    public async Task<List<int>> GetUserIdsFromFbIds(IEnumerable<int> fbIds)
    {
      var result = new List<int>();

      foreach (var fbId in fbIds)
      {
        var userDb = await _context.UsersDb.FirstOrDefaultAsync(x => x.FacebookId == fbId);
        if (userDb != null)
          result.Add(userDb.UserId);
      }
      return result;
      //var dataTable = new DataTable();
      //dataTable.Columns.Add("id", typeof(int));
      //foreach (var id in ids)
      //{
      //  dataTable.Rows.Add(id);
      //}
      //var vkIdTable = new SqlParameter
      //{
      //  ParameterName = "@FbIdTable",
      //  TypeName = "dbo.idTable",
      //  Value = dataTable
      //};
      //return _context.UserIds.FromSql($"select * from dbo.[getRecommendedFromUsersID](@FbIdTable)", vkIdTable)
      //  .ToList();
    }

    public async Task<UserDto> GetUserByExternalUserIdAsync(int externalUserId, SocialNetType socialType)
    {
      UserDb user;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.FacebookId == externalUserId);
          break;
        case SocialNetType.Vk:
          user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.VkId == externalUserId);
          break;
        case SocialNetType.Nothing:
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        default:
          user = null;
          break;
      }
      return user?.ToUserDto();
    }

    public async Task<long> GetExternalUserIdAsync(int userId, SocialNetType socialType)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return 0;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          return user.FacebookId;
        case SocialNetType.Vk:
          return user.VkId;
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        case SocialNetType.Nothing:
        default:
          return 0;
      }
    }

    public async Task<bool> IsUserIdExistAsync(int userId)
    {
      return await _context.UsersDb.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByPhoneAsync(string phone)
    {
      return await _context.UsersDb.AsNoTracking().AnyAsync(x => x.PhoneNumber == phone);
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
      return await _context.UsersDb.AsNoTracking().AnyAsync(x=>x.Email == email);
    }

    public async Task<UserContactsDto> GetUserSocialsAsync(int userId)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return new UserContactsDto();

      return new UserContactsDto
      {
        VkId = user.VkId,
        FacebookId = user.FacebookId
      };
    }
  }
}