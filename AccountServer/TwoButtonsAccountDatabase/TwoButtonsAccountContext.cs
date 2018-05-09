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
  }
}