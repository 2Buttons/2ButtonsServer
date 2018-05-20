using AccountData.Account.Entities;
using AccountData.Account.Entities.FunctionEntities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Account
{
  public class TwoButtonsAccountContext : DbContext
  {
    public DbSet<UserDb> UsersDb { get; set; }

    // functions
    public DbSet<UserIdDb> UserIds { get; set; }


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

        entity.Property(e => e.FacebookId).HasDefaultValueSql("((0))");

        entity.Property(e => e.FacebookToken).HasMaxLength(256);

        entity.Property(e => e.RoleType).HasDefaultValueSql("((0))");

        entity.Property(e => e.VkId).HasDefaultValueSql("((0))");

        entity.Property(e => e.VkToken).HasMaxLength(256);
      });
    }
  }
}