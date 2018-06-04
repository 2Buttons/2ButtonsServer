using Microsoft.EntityFrameworkCore;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<CommentDb> CommentsDb { get; set; }

    public virtual DbQuery<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbQuery<NotificationDb> NotificationsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}