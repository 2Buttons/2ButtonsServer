using AccountServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Data
{
  public class TwoButtonsAccountContext1 : DbContext
  {
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<TokenDb> Tokens { get; set; }
    public virtual DbSet<Clients> Clients { get; set; }
    public virtual DbSet<Tokens> Tokens { get; set; }
    public virtual DbSet<Users> Users { get; set; }

    public TwoButtonsAccountContext1(DbContextOptions<TwoButtonsAccountContext1> options)
      : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Clients>(entity =>
      {
        entity.HasKey(e => e.ClientId);

        entity.Property(e => e.AllowedOrigin).HasMaxLength(50);

        entity.Property(e => e.ApplicationType).HasDefaultValueSql("((0))");

        entity.Property(e => e.RefreshTokenLifeTime).HasDefaultValueSql("((0))");

        entity.Property(e => e.Secret).HasMaxLength(50);
      });

      modelBuilder.Entity<Tokens>(entity =>
      {
        entity.HasKey(e => e.TokenId);

        entity.Property(e => e.RefreshToken)
          .IsRequired()
          .HasMaxLength(100);
      });

      modelBuilder.Entity<Users>(entity =>
      {
        entity.HasKey(e => e.UserId);

        entity.Property(e => e.UserId).ValueGeneratedNever();

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