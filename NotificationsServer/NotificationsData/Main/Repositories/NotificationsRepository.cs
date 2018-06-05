using System;
using System.Collections.Generic;
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

    public async Task<List<NotificationDb>> GetNotifications(int userId)
    {
      return await _context.NotificationsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNotifications({userId})").ToListAsync();
    }

    public async Task<bool> UpdateLastSeen(int userId)
    {
      var user = await _context.UserInfoDb.FirstOrDefaultAsync(x => x.UserId == userId);
      user.LastNotsSeenDate = DateTime.UtcNow;
      _context.Update(user);
      return await _context.SaveChangesAsync() > 0;
    }


  }
}