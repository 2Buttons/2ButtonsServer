using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwoButtonsAccountDatabase.Entities;

namespace TwoButtonsAccountDatabase
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



    public static bool TryAddUser(string phone, string password, int role, out int userId)
    {

      var userIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output,
      };

      var registrationDate = DateTime.UtcNow;

      try
      {
        db.Database.ExecuteSqlCommand(
          $"addUser {login}, {password}, {age}, {sex}, {city}, {phone}, {description}, {fullAvatarLink}, {smallAvatarLink}, {role}, {registrationDate}, {userIdDb} OUT");
        userId = (int) userIdDb.Value;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userId = -1;
      return false;

    }


    public async Task<int> GetUserIdByEmail(string email, string passwordHash)
    {
      var user = await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
      return user?.UserId ?? 0;
    }

    public async Task<int> GetUserIdByPhone(string phone, string passwordHash)
    {
        var user = await _context.Users.AsNoTracking()
          .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PasswordHash == passwordHash);
        return user?.UserId ?? 0;
    }

    public async Task<int> GetUserIdByPhoneExternalUserId(int externalUserId, SocialNetType socialType)
    {
      switch (socialType)
      {
        
        case SocialNetType.Facebook:
          return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.FacebookId == externalUserId)?.UserId ?? 0;
        case SocialNetType.VK:
          break;
        case SocialNetType.Nothing:
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        default:
          return 0;
      }
      var user = await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x. == phone && x.PasswordHash == )?.UserId ?? 0;
      return user;
    }

    public async Task<int> GetExternalUserId(int userId, SocialNetType socialType)
    {
      var user =  await _context.Users.FindAsync(userId);
      if (user == null) return 0;
      switch (socialType)
      {
        
        case SocialNetType.Facebook:
          return user.FacebookId;
        case SocialNetType.VK:
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

    public async Task<bool> IsUserIdExist(int userId)
    {
      return await _context.Users.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByPhone(int userId)
    {
      return await _context.Users.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByEmail(int userId)
    {
      return await _context.Users.FindAsync(userId) != null;
    }

    public async Task<RoleType> GetUserRole(int userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user != null)
        return (RoleType)user.RoleType;
      return RoleType.Guest;
    }


    public async Task<List<SocialNetType>> GetUserContacts(int userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return new List<SocialNetType>();

      var result = new List<SocialNetType>();
      if (user.VkId != 0) result.Add(SocialNetType.VK);
      if (user.FacebookId != 0) result.Add(SocialNetType.Facebook);

      return result;
    }
  }
}