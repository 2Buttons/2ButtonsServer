using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.Repositories
{
  public class NotificationsRepository
  {
    private readonly TwoButtonsContext _db;

    public NotificationsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<List<NotificationDb>> GetNotifications(int userId)
    {
      try
      {
        return await _db.NotificationsDb.AsNoTracking()
          .FromSql($"select * from dbo.getNotifications({userId})").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<NotificationDb>();
    }

    public async Task<bool> UpdateNotsDate(int userId)
    {
      try
      {
        var newLastNots = DateTime.UtcNow;
        return await _db.Database.ExecuteSqlCommandAsync($"updateNotsDate {userId}, {newLastNots}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}