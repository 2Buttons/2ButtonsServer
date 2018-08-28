using CommonLibraries.Entities.Acccount;
using Microsoft.EntityFrameworkCore;

namespace BotsData.Contexts
{
  public class TwoButtonsAccountContext : DbContext
  {

    public DbSet<UserEntity> UserEntities { get; set; }

    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options) : base(options)
    {
    }
  }
}
