using System;
using System.Collections.Generic;
using System.Linq;
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

    public bool TryGetNotifications(int userId,
      out IEnumerable<NotificationDb> notifications)
    {
      try
      {
        notifications = _db.NotificationsDb
          .FromSql($"select * from dbo.getNotifications({userId})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      notifications = new List<NotificationDb>();
      return false;
    }

    public bool TryUpdateNotsDate(int userId)
    {
      try
      {
        var newLastNots = DateTime.UtcNow;
        _db.Database.ExecuteSqlCommand($"updateNotsDate {userId}, {newLastNots}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}