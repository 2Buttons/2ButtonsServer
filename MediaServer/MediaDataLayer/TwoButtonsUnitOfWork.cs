using System;
using MediaDataLayer.Repositories;

namespace MediaDataLayer
{
  public class TwoButtonsUnitOfWork : IDisposable
  {
    private readonly TwoButtonsContext _db;

    private AccountRepository _accountRepository;
    private QuestionRepository _questionRepository;

    public AccountRepository Accounts => _accountRepository ?? (_accountRepository = new AccountRepository(_db));

    public QuestionRepository Questions => _questionRepository ?? (_questionRepository = new QuestionRepository(_db));

    public TwoButtonsUnitOfWork(TwoButtonsContext db)
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