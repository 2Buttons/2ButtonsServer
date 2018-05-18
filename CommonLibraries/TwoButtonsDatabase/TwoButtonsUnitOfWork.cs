using System;
using TwoButtonsDatabase.Repositories;

namespace TwoButtonsDatabase
{
  public class TwoButtonsUnitOfWork : IDisposable
  {
    private readonly TwoButtonsContext _db;

    private AccountRepository _accountRepository;
    private CommentsRepository _commentsRepository;
    private FollowersRepository _followersRepository;
    private ComplaintsRepository _moderatorRepository;
    private NewsQuestionsRepository _newsRepository;
    private NotificationsRepository _notificationsRepository;
    private QuestionRepository _questionRepository;
    private RecommendedPeopleRepository _recommendedSubscribersRepository;
    private TagsRepository _tagsRepository;
    private UserQuestionsRepository _userQuestionsRepository;


    public AccountRepository Accounts => _accountRepository ?? (_accountRepository = new AccountRepository(_db));

    public CommentsRepository Comments => _commentsRepository ?? (_commentsRepository = new CommentsRepository(_db));

    public FollowersRepository Followers => _followersRepository ??(_followersRepository = new FollowersRepository(_db));

    public ComplaintsRepository Complaints => _moderatorRepository ??(_moderatorRepository = new ComplaintsRepository(_db));

    public NewsQuestionsRepository News => _newsRepository ?? (_newsRepository = new NewsQuestionsRepository(_db));

    public NotificationsRepository Notifications => _notificationsRepository ?? (_notificationsRepository = new NotificationsRepository(_db));

    public QuestionRepository Questions => _questionRepository ?? (_questionRepository = new QuestionRepository(_db));

    public RecommendedPeopleRepository RecommendedPeople => _recommendedSubscribersRepository ??(_recommendedSubscribersRepository =new RecommendedPeopleRepository(_db));

    public TagsRepository Tags => _tagsRepository ?? (_tagsRepository = new TagsRepository(_db));

    public UserQuestionsRepository UserQuestions => _userQuestionsRepository ??(_userQuestionsRepository = new UserQuestionsRepository(_db));


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