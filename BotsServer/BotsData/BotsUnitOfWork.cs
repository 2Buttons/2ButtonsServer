using System;
using System.Collections.Generic;
using System.Text;

namespace BotsData
{
    public class BotsUnitOfWork : IDisposable
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;


    public UserInfoRepository UsersInfo => _usersInfoRepository ?? (_usersInfoRepository = new UserInfoRepository(_dbMain));

    public AuthorizationUnitOfWork(TwoButtonsContext dbMain, TwoButtonsAccountContext dbAccount)
    {
      _dbMain = dbMain;
      _dbAccount = dbAccount;
    }


    #region IDisposable

    private bool _disposed;

    public virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          _dbAccount.Dispose();
          _dbMain.Dispose();
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
