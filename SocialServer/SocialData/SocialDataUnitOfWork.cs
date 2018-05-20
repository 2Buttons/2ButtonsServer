using System;
using SocialData.Account;
using SocialData.Account.Repostirories;
using SocialData.Main;
using SocialData.Main.Repositories;

namespace SocialData
{
  public class SocialDataUnitOfWork : IDisposable
  {
    private readonly TwoButtonsAccountContext _dbAccount;
    private readonly TwoButtonsContext _dbMain;

    private FollowersRepository _followersRepository;
    private RecommendedPeopleRepository _recommendedSubscribersRepository;
    private UserRepository _userRepository;


    public FollowersRepository Followers => _followersRepository ??(_followersRepository = new FollowersRepository(_dbMain));
    public RecommendedPeopleRepository RecommendedPeople => _recommendedSubscribersRepository ??(_recommendedSubscribersRepository =new RecommendedPeopleRepository(_dbMain));
    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_dbAccount));

    public SocialDataUnitOfWork(TwoButtonsContext dbMain, TwoButtonsAccountContext dbAccount)
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
          _dbMain.Dispose();
          _dbAccount.Dispose();
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