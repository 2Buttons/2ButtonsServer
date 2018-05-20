using System;
using AuthorizationData.Account;
using AuthorizationData.Account.Repostirories;
using AuthorizationData.Main;

namespace AuthorizationData
{
  public class AuthorizationUnitOfWork : IDisposable
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;

    //private ClientRepository _clientRepository;
    private TokenRepository _tokenRepository;
    private UserRepository _userRepository;

    public TokenRepository Tokens => _tokenRepository ?? (_tokenRepository = new TokenRepository(_dbAccount));
    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_dbMain, _dbAccount));

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