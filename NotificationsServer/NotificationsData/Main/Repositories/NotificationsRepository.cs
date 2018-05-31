using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main.Repositories
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
        return await _db.NotificationsDb.AsNoTracking()
          .FromSql($"select * from dbo.getNotifications({userId})").ToListAsync() ?? new List<NotificationDb>();
    }

    public async Task<bool> UpdateNotsDate(int userId)
    {
        var newLastNots = DateTime.UtcNow;
        return await _db.Database.ExecuteSqlCommandAsync($"updateNotsDate {userId}, {newLastNots}") >0;
    }
  }
}