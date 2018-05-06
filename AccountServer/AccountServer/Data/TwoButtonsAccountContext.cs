using AccountServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Data
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