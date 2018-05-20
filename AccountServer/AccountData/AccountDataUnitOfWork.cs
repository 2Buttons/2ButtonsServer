using System;
using AccountData.Account;
using AccountData.Account.Repostirories;
using AccountData.Main;
using AccountData.Main.Repositories;

namespace AccountData
{
  public class AccountDataUnitOfWork
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;

    private AccountRepository _accountRepository;
    private UserRepository _userRepository;

    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_dbAccount));
    public AccountRepository Accounts => _accountRepository ?? (_accountRepository = new AccountRepository(_dbMain));

    public AccountDataUnitOfWork(TwoButtonsContext dbMain, TwoButtonsAccountContext dbAccount)
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