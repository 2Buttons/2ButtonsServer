using Microsoft.EntityFrameworkCore;
using MonitoringData.Entities;

namespace MonitoringData
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<UrlMonitoringDb> UrlMonitoringsDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}