using AccountServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Repositories
{
  public class TwoButtonsAccountContext : DbContext
  {
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<TokenDb> Tokens { get; set; }

    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options)
      : base(options)
    {
    }
  }
}