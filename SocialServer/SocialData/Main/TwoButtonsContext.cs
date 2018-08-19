
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;
using SocialData.Main.Entities;
using SocialData.Main.Entities.Followers;
using SocialData.Main.Entities.Recommended;


namespace SocialData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbQuery<FollowingQuery> Followings { get; set; }

    public virtual DbQuery<RecommendedStrangersDb> RecommendedStrangersDb { get; set; }

    public virtual DbQuery<RecommendedFromFollowsDb> RecommendedFromFollowsDb { get; set; }
    public virtual DbQuery<RecommendedFromFollowersDb> RecommendedFromFollowersDb { get; set; }
    public virtual DbQuery<RecommendedFromUsersIdDb> RecommendedFromUsersIdsDb { get; set; }

    public virtual DbSet<FollowingEntity>  UserRelationshipEntities { get; set; }
    public virtual DbSet<StatisticsEntity> StatisticsEntities { get; set; }
    public virtual DbSet<UserInfoEntity> UserInfoEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<FollowingEntity>().HasKey(x => new { x.UserId, x.FollowingId });
    }
  }
}