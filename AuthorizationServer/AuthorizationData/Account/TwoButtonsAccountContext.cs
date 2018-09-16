using AuthorizationData.Account.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account
{
  public class TwoButtonsAccountContext : DbContext
  {
    public DbSet<TokenEntity> TokenEntities { get; set; }
    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<SocialEntity> SocialEntities { get; set; }
    public DbSet<EmailEntity> EmailEntities { get; set; }


    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.Entity<TokenDb>(entity =>
      //{
      //  entity.HasKey(e => e.TokenId);

      //  entity.Property(e => e.RefreshToken)
      //    .IsRequired();

      //  entity.Property(e => e.AllowedOrigin).HasMaxLength(50);

      //  entity.Property(e => e.ApplicationType).HasDefaultValueSql("((0))");
      //});

      //modelBuilder.Entity<UserDb>(entity =>
      //{
      //  entity.HasKey(e => e.UserId);

      //  entity.Property(e => e.AccessFailedCount).HasDefaultValueSql("((0))");

      //  entity.Property(e => e.Email).HasMaxLength(256);

      //  entity.Property(e => e.RoleType).HasDefaultValueSql("((0))");
      //});
    }
  }
}