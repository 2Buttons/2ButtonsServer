using Microsoft.EntityFrameworkCore;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbSet<NotificationDb> NotificationsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}