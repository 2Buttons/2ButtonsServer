using System;
using System.Threading.Tasks;
using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main.Repositories
{
  public class AccountRepository
  {
    private readonly TwoButtonsContext _db;

    public AccountRepository(TwoButtonsContext db)
    {
      _db = db;
    }


    public async Task<UserInfoDb> GetUserInfo(int userId, int userPageId)
    {
      try
      {
        var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
                     .FirstOrDefaultAsync() ??
                   new UserInfoDb();

        if (userId != userPageId)
          if (user.YouFollowed)
            UpdateVisits(userId, userPageId);
        return user;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new UserInfoDb();
    }

    public async Task<UserStatisticsDb> GetUserStatistics(int userId)
    {
      try
      {
        return await _db.UserStatisticsDb
                 .FromSql($"select * from dbo.getUserStatistics({userId})").FirstOrDefaultAsync() ??
               new UserStatisticsDb();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new UserStatisticsDb();
    }

    public async Task<bool> UpdateVisits(int userId, int getUserId)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateVisits {userId}, {getUserId}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

  }
}