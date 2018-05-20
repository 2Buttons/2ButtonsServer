using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Main
{
  public class TwoButtonsContext : DbContext
  {
    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}