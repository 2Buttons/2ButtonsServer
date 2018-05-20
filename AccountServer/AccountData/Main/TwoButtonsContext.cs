using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbSet<UserStatisticsDb> UserStatisticsDb { get; set; }
    public virtual DbSet<NotificationDb> NotificationsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}