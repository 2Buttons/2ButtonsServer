using System;
using System.Collections.Generic;
using System.Text;
using BotsData.Contexts;
using BotsData.Repositories;

namespace BotsData
{
    public class BotsUnitOfWork : IDisposable
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;


    private QuestionsRepository _questionRepository;

    public QuestionsRepository  QuestionRepository => _questionRepository ?? (_questionRepository = new QuestionsRepository(_dbMain, _dbAccount));

    private BotsRepository   _botsRepository;

    public BotsRepository BotsRepository => _botsRepository ?? (_botsRepository = new BotsRepository(_dbMain, _dbAccount));

    public BotsUnitOfWork(TwoButtonsContext dbMain, TwoButtonsAccountContext dbAccount)
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
