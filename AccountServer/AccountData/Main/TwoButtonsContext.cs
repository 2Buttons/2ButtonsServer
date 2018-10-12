using AccountData.Main.Queries;
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;


namespace AccountData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbQuery<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbQuery<UserStatisticsDb> UserStatisticsDb { get; set; }
    public virtual DbQuery<NotificationQuery> NotificationsDb { get; set; }

    public virtual DbSet<UserInfoEntity> UserInfoEntities { get; set; }
    public virtual DbSet<FollowingEntity> FollowingEntities { get; set; }
    public virtual DbSet<StatisticsEntity> StatisticsEntities { get; set; }
    public virtual DbSet<FeedbackEntity> FeedbackEntities { get; set; }
    public virtual DbSet<CityEntity> CityEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<FollowingEntity>().HasKey(x => new { x.UserId, x.FollowingId });
    }
  }


}