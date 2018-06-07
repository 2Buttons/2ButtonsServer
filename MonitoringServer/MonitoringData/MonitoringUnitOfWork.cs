using System;
using MonitoringData.Repositories;

namespace MonitoringData
{
  public class MonitoringUnitOfWork : IDisposable
  {
    private readonly TwoButtonsContext _db;

    private MonitoringRepository _monitoringRepository;

    public MonitoringRepository Monitorings => _monitoringRepository ??
                                                (_monitoringRepository = new MonitoringRepository(_db));

    public MonitoringUnitOfWork(TwoButtonsContext db)
    {
      _db = db;
    }

    #region IDisposable

    private bool _disposed;

    public virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing) _db.Dispose();
        _disposed = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}