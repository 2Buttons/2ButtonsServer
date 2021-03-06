﻿using System;
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
    private CityRepository _cityRepository;

    private FeedbackRepository _feedbackRepository;
    private NotificationsRepository _notificationsRepository;
    private UserInfoRepository _userInfoRepository;
    private UserRepository _userRepository;

    public FeedbackRepository Feedbacks => _feedbackRepository ??
                                           (_feedbackRepository = new FeedbackRepository(_dbMain));

    public CityRepository Cities => _cityRepository ?? (_cityRepository = new CityRepository(_dbMain));

    public UserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_dbAccount));

    public UserInfoRepository UsersInfo => _userInfoRepository ??
                                           (_userInfoRepository = new UserInfoRepository(_dbMain));

    public NotificationsRepository Notifications => _notificationsRepository ??
                                                    (_notificationsRepository = new NotificationsRepository(_dbMain));

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