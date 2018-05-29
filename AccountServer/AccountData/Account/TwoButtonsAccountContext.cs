using AccountData.Account.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Account
{
  public class TwoButtonsAccountContext : DbContext
  {
    public DbSet<UserDb> UsersDb { get; set; }
    public DbSet<SocialDb> SocialsDb { get; set; }


    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
     modelBuilder.Entity<UserDb>(entity =>
      {
        entity.HasKey(e => e.UserId);

        entity.Property(e => e.AccessFailedCount).HasDefaultValueSql("((0))");

        entity.Property(e => e.Email).HasMaxLength(256);

       
      });
    }
  }
}