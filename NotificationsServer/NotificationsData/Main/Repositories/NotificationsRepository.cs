using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main.Repositories
{
  public class NotificationsRepository
  {
    private readonly TwoButtonsContext _context;

    public NotificationsRepository(TwoButtonsContext context)
    {
      _context = context;
    }

    public async Task<int> GetUserIdForComment(int commentId)
    {
      return (await _context.CommentsDb.FindAsync(commentId))?.UserId ?? -1;
    }

    public List<NotificationDb> GetNotifications(int userId)
    {
      var p = _context.Query<NotificationDb>().FromSql($"select * from dbo.getNotifications({userId})");
      var t = p;
        return  _context.Query<NotificationDb>()
          .FromSql($"select * from dbo.getNotifications({userId})").ToList() ?? new List<NotificationDb>();
    }

    public async Task<bool> UpdateNotsDate(int userId)
    {
        var newLastNots = DateTime.UtcNow;
        return await _context.Database.ExecuteSqlCommandAsync($"updateNotsDate {userId}, {newLastNots}") >0;
    }

   
  }
}