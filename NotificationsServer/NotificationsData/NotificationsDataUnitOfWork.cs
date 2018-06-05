using System;
using NotificationsData.Main;
using NotificationsData.Main.Repositories;

namespace NotificationsData
{
  public class NotificationsDataUnitOfWork : IDisposable
  {
    private readonly TwoButtonsContext _dbMain;
    private NotificationsRepository _notificationsRepository;

    private UserInfoRepository _userInfoRepository;

    public UserInfoRepository UsersInfo => _userInfoRepository ??
                                           (_userInfoRepository = new UserInfoRepository(_dbMain));

    public NotificationsRepository Notifications => _notificationsRepository ??
                                                    (_notificationsRepository = new NotificationsRepository(_dbMain));

    public NotificationsDataUnitOfWork(TwoButtonsContext dbMain)
    {
      _dbMain = dbMain;
    }

    #region IDisposable

    private bool _disposed;

    public virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
         
          _dbMain?.Dispose();
        }
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