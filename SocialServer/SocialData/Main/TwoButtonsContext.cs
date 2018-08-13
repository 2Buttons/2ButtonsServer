
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;
using SocialData.Main.Entities;
using SocialData.Main.Entities.Followers;
using SocialData.Main.Entities.Recommended;


namespace SocialData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<FollowerDb> FollowerDb { get; set; }
    public virtual DbSet<FollowToDb> FolloToDb { get; set; }
    public virtual DbSet<NewFollowersDb> NewFollowersDb { get; set; }

    //public virtual DbSet<RecommendedFromContactsDb> RecommendedFromContactsDb { get; set; }
    public virtual DbSet<RecommendedStrangersDb> RecommendedStrangersDb { get; set; }

    public virtual DbSet<RecommendedFromFollowsDb> RecommendedFromFollowsDb { get; set; }
    public virtual DbSet<RecommendedFromFollowersDb> RecommendedFromFollowersDb { get; set; }
    public virtual DbSet<RecommendedFromUsersIdDb> RecommendedFromUsersIdsDb { get; set; }

    public virtual DbSet<UserRelationshipEntity>  UserRelationshipEntities { get; set; }
    public virtual DbSet<StatisticsEntity> StatisticsEntities { get; set; }
    public virtual DbSet<UserInfoEntity> UserInfoEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}