using System;
using NotificationsData.Account;
using NotificationsData.Account.Repostirories;
using NotificationsData.Main;
using NotificationsData.Main.Repositories;

namespace NotificationsData
{
  public class NotificationsDataUnitOfWork //: IDisposable
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;

    private UserInfoRepository _userInfoRepository;
    private NotificationsRepository _notificationsRepository;
    private UserRepository _userRepository;


    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_dbAccount));

    public UserInfoRepository UsersInfo => _userInfoRepository ?? (_userInfoRepository = new UserInfoRepository(_dbMain));

    public NotificationsRepository Notifications => _notificationsRepository ??
                                                    (_notificationsRepository = new NotificationsRepository(_dbMain));

    public NotificationsDataUnitOfWork(TwoButtonsContext dbMain, TwoButtonsAccountContext dbAccount)
    {
      _dbMain = dbMain;
      _dbAccount = dbAccount;
    }


    //#region IDisposable

    //private bool _disposed;

    //public virtual void Dispose(bool disposing)
    //{
    //  if (!_disposed)
    //  {
    //    if (disposing)
    //    {
    //      _dbAccount?.Dispose();
    //      _dbMain?.Dispose();
    //    }
    //    _disposed = true;
    //  }
    //}

    //public void Dispose()
    //{
    //  Dispose(true);
    //  GC.SuppressFinalize(this);
    //}

    //#endregion
  }
}