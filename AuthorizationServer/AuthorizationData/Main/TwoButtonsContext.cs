using AuthorizationData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Main
{
  public class TwoButtonsContext : DbContext
  {

    public DbSet<UserInfoDb> UsersInfoDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}