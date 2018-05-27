using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities.UserQuestions;

namespace QuestionsData.Repositories
{
  public class UserQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public UserQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddRecommendedQuestion(int userToId, int pageUserId, int questionId)
    {
      var recommendedDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"addTag {userToId}, {pageUserId}, {questionId}, {recommendedDate}") > 0;
    }

    public async Task<List<UserAskedQuestionDb>> GetUserAskedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserAskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserAskedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {isAnonimus})")
               .OrderBy(predicate).Skip(offset).Take(count).ToListAsync() ?? new List<UserAskedQuestionDb>();
    }

    public async Task<List<UserAnsweredQuestionDb>> GetUserAnsweredQuestions(int userId, int pageUserId, int offset,
      int count)
    {
      var isAnonimus = userId == pageUserId ? 1 : 0;

      return await _db.UserAnsweredQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {isAnonimus})")
               .OrderBy(x => x.QuestionAddDate).Skip(offset).Take(count).ToListAsync() ??
             new List<UserAnsweredQuestionDb>();
    }

    public async Task<List<UserFavoriteQuestionDb>> GetUserFavoriteQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserFavoriteQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserFavoriteQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {isAnonimus})")
               .OrderBy(predicate).Skip(offset).Take(count).ToListAsync() ?? new List<UserFavoriteQuestionDb>();
    }

    public async Task<List<UserCommentedQuestionDb>> GetUserCommentedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserCommentedQuestionDb, object>> predicate)
    {
      return await _db.UserCommentedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId})").OrderBy(predicate)
               .Skip(offset).Take(count).ToListAsync() ?? new List<UserCommentedQuestionDb>();
    }

    public async Task<List<TopQuestionDb>> GeTopQuestions(int userId, bool isOnlyNew, int offset, int count,
      DateTime topAfterDate, Expression<Func<TopQuestionDb, object>> predicate)
    {
      return await _db.TopQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getTop({userId}, {topAfterDate}, {isOnlyNew})").OrderBy(predicate)
               .Skip(offset).Take(count).ToListAsync() ?? new List<TopQuestionDb>();
    }

    public async Task<List<AskedQuestionDb>> GetAskedQuestions(int userId, int pageUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.AskedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId},   {isAnonimus})")
               .OrderBy(predicate).Skip(offset).Take(count).ToListAsync() ?? new List<AskedQuestionDb>();
    }

    public async Task<List<RecommendedQuestionDb>> GetRecommendedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<RecommendedQuestionDb, object>> predicate)
    {
      return await _db.RecommendedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserRecommendedQuestions({userId})").OrderBy(predicate).Skip(offset)
               .Take(count).ToListAsync() ?? new List<RecommendedQuestionDb>();
    }

    public async Task<List<ChosenQuestionDb>> GetChosenQuestions(int userId, int pageUserId, int offset,
      int count)
    {
      return await _db.ChosenQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getNotAnsweredQuestions({userId})")
               .OrderBy(x=>((5 * x.Likes - 3 * x.Dislikes) > 10 ? (5 * x.Likes - 3 * x.Dislikes) : (15+5 * x.Likes - 3 * x.Dislikes)) - (x.FirstAnswers + x.SecondAnswers)).Skip(offset)
               .Take(count).ToListAsync() ?? new List<ChosenQuestionDb>();
    }

    public async Task<List<LikedQuestionDb>> GetLikedQuestions(int userId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate)
    {
      return await _db.LikedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getUserLikedQuestions({userId})")
               .OrderBy(predicate).Skip(offset).Take(count).ToListAsync() ?? new List<LikedQuestionDb>();
    }

    public async Task<List<SavedQuestionDb>> GetSavedQuestions(int userId, int offset, int count,
      Expression<Func<SavedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.SavedQuestionsDb.AsNoTracking()
               .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {userId}, {isAnonimus})")
               .OrderBy(predicate).Skip(offset).Take(count).ToListAsync() ?? new List<SavedQuestionDb>();
    }
  }
}