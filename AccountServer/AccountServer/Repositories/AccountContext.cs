using AccountServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Repositories
{
  public class AccountContext : DbContext
  {
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<TokenDb> Tokens { get; set; }

    public AccountContext(DbContextOptions<AccountContext> options)
      : base(options)
    {
    }
  }
}