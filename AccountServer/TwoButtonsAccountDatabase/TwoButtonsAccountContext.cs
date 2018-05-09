using AccountServer;
using AccountServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using TwoButtonsAccountDatabase.Entities;

namespace TwoButtonsAccountDatabase
{
  public class TwoButtonsAccountContext : DbContext
  {
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<TokenDb> Tokens { get; set; }
    public DbSet<UserDb> Users { get; set; }

    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ClientDb>(entity =>
      {
        entity.HasKey(e => e.ClientId);

        entity.Property(e => e.AllowedOrigin).HasMaxLength(50);

        entity.Property(e => e.ApplicationType).HasDefaultValueSql("((0))");

        entity.Property(e => e.RefreshTokenLifeTime).HasDefaultValueSql("((0))");

        entity.Property(e => e.Secret).HasMaxLength(50);
      });

      modelBuilder.Entity<TokenDb>(entity =>
      {
        entity.HasKey(e => e.TokenId);

        entity.Property(e => e.RefreshToken)
          .IsRequired()
          .HasMaxLength(100);
      });

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