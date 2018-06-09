using System;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Repositories;

namespace QuestionsData
{
  public class QuestionsUnitOfWork : IDisposable
  {
    private readonly TwoButtonsContext _db;

    private CommentsRepository _commentsRepository;
    private ComplaintsRepository _moderatorRepository;
    private NewsQuestionsRepository _newsRepository;
    private QuestionRepository _questionRepository;
    private TagsRepository _tagsRepository;
    private UserQuestionsRepository _userQuestionsRepository;

    public CommentsRepository Comments => _commentsRepository ?? (_commentsRepository = new CommentsRepository(_db));

    public ComplaintsRepository Complaints => _moderatorRepository ??(_moderatorRepository = new ComplaintsRepository(_db));

    public NewsQuestionsRepository News => _newsRepository ?? (_newsRepository = new NewsQuestionsRepository());

    public QuestionRepository Questions => _questionRepository ?? (_questionRepository = new QuestionRepository(_db));

    public TagsRepository Tags => _tagsRepository ?? (_tagsRepository = new TagsRepository(_db));

    public UserQuestionsRepository UserQuestions => _userQuestionsRepository ??(_userQuestionsRepository = new UserQuestionsRepository(_db));


    public QuestionsUnitOfWork(TwoButtonsContext db, DbContextOptions<TwoButtonsContext> dbOptions)
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