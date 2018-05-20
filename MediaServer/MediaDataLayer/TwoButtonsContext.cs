using Microsoft.EntityFrameworkCore;

namespace MediaDataLayer
{
  public class TwoButtonsContext : DbContext
  {
    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}