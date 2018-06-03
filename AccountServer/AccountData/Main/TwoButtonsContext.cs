using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbQuery<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbQuery<UserStatisticsDb> UserStatisticsDb { get; set; }
    public virtual DbQuery<NotificationDb> NotificationsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}