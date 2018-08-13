using AuthorizationData.Main.Entities;
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Main
{
  public class TwoButtonsContext : DbContext
  {

    public DbSet<UserInfoDb> UsersInfoDb { get; set; }
    public DbSet<UserInfoEntity> UserInfoEntities { get; set; }
    public DbSet<StatisticsEntity> StatisticsEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}