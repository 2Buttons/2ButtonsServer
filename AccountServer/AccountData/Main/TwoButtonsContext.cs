using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main
{
  public class TwoButtonsContext : DbContext
  {
    //for functions and prosedures


    public virtual DbSet<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbSet<UserStatisticsDb> UserStatisticsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}