using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.Repositories
{
  public class NewsQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public NewsQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<List<NewsAskedQuestionsDb>> GetNewsAskedQuestions(int userId, int offset, int count)
    {
      return _db.NewsAskedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getNewsAskedQuestions({userId})")
               .Skip(offset).Take(count).ToList();
    }

    public async Task<List<NewsAnsweredQuestionsDb>> GetNewsAnsweredQuestions(int userId, int offset, int count)
    {
      return  _db.NewsAnsweredQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getNewsAnsweredQuestions({userId})").Skip(offset).Take(count)
               .ToList();
    }

    public async Task<List<NewsFavoriteQuestionsDb>> GetNewsFavoriteQuestions(int userId, int offset, int count)
    {
      return  _db.NewsFavoriteQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getNewsFavoriteQuestions({userId})").Skip(offset).Take(count)
               .ToList();
    }

    public async Task<List<NewsCommentedQuestionsDb>> GetNewsCommentedQuestions(int userId, int offset, int count)
    {
      return  _db.NewsCommentedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getNewsCommentedQuestions({userId})").Skip(offset).Take(count)
               .ToList();
    }

    public async Task<List<NewsRecommendedQuestionDb>> GetNewsRecommendedQuestions(int userId, int offset, int count)
    {
      return  _db.NewsRecommendedQuestionDb.AsNoTracking()
               .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})").Skip(offset).Take(count)
               .ToList();
    }
  }
}