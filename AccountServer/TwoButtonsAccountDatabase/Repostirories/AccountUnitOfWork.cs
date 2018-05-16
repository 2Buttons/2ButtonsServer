using System;

namespace TwoButtonsAccountDatabase.Repostirories
{
  public class AccountUnitOfWork : IDisposable
  {
    private readonly TwoButtonsAccountContext _db;

    //private ClientRepository _clientRepository;
    private TokenRepository _tokenRepository;
    private UserRepository _userRepository;

    public TokenRepository Tokens => _tokenRepository ?? (_tokenRepository = new TokenRepository(_db));
   // public ClientRepository Clients => _clientRepository ?? (_clientRepository = new ClientRepository(_db));
    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_db));

    public AccountUnitOfWork(TwoButtonsAccountContext db)
    {
      _db = db;
    }


    #region IDisposable

    private bool _disposed;

    public virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
          _db.Dispose();
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