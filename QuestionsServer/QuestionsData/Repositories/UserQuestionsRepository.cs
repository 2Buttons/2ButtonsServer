using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsData.Queries.UserQuestions;

namespace QuestionsData.Repositories
{
  public class UserQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public UserQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    //public async Task<List<UserIdValidDto>> CheckIdsValid(IEnumerable<int> ids)
    //{
    //  return await _db.UserEntities.Where(x=> ids.Contains(x.UserId)x.UserId.Select(x => new UserIdValidDto { UserId=x.UserId, IsValid= ids.Contains(x.UserId)}).ToListAsync();
    //}

    public async Task<bool> AddRecommendedQuestion(int userToId, int userFromId, int questionId)
    {
      if (_db.UserEntities.All(x => x.UserId != userToId)) return false;

      var recommendedDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"addRecommendedQuestion {userToId}, {userFromId}, {questionId}, {recommendedDate}") > 0;
    }

    public async Task<List<UserAskedQuestionDb>> GetUserAskedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserAskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserAskedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserAnsweredQuestionDb>> GetUserAnsweredQuestions(int userId, int pageUserId, int offset,
      int count)
    {
      var isAnonimus = userId == pageUserId ? 1 : 0;

      return await _db.UserAnsweredQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(x => x.QuestionAddDate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserFavoriteQuestionDb>> GetUserFavoriteQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserFavoriteQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserFavoriteQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserCommentedQuestionDb>> GetUserCommentedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserCommentedQuestionDb, object>> predicate)
    {
      return await _db.UserCommentedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId})").OrderByDescending(predicate)
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<TopQuestionDb>> GeTopQuestions(int userId, bool isOnlyNew, int offset, int count,
      DateTime topAfterDate, Expression<Func<TopQuestionDb, object>> predicate)
    {
      return await _db.TopQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getTop({userId}, {topAfterDate}, {isOnlyNew})").OrderByDescending(predicate)
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<AskedQuestionDb>> GetAskedQuestions(int userId, int pageUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.AskedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId},   {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<RecommendedQuestionDb>> GetRecommendedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<RecommendedQuestionDb, object>> predicate)
    {
      return await _db.RecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserRecommendedQuestions({userId})").OrderByDescending(predicate).Skip(offset)
        .Take(count).ToListAsync();
    }

    //public async Task<List<RecommendedQuestionDto>> GetPersonalRecommendedQuestions(int userId, int offset, int count,
    //  Expression<Func<RecommendedQuestionDto, object>> predicate)
    //{
    //  var questions = _db.RecommendedQuestionsDb.AsNoTracking()
    //    .FromSql($"select * from dbo.getUserRecommendedQuestions({userId})").OrderByDescending(predicate).Skip(offset)
    //    .Take(count).Select(
    //    y => new RecommendedQuestionDto

    //    {
    //      QuestionId = y.QuestionId,
    //      UserId = y.UserId,
    //      Condition = y.Condition,
    //      IsAnonymity = y.IsAnonymity,
    //      IsAudience = y.IsAudience,
    //      QuestionType = y.QuestionType,
    //      QuestionAddDate = y.QuestionAddDate,
    //      BackgroundImageLink = y.BackgroundImageLink,
    //      Answers = y.Answers,
    //      Likes = y.Likes,
    //      Dislikes = y.Dislikes,
    //      IsDeleted = y.IsDeleted
    //    }

    //  ).OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();

    //  // var questionUIds = _db.QuestionIdQueries.Intersect(questions.Select(x => new QuestionIdQuery { QuestionId = x.QuestionId}));// = questions.Select(x => new RecommendedUserQuestionQuery {QuestionId = x.QuestionId, UserId = userId}).ToList();

    //  for (var i = 0; i < questions.Count; i++)
    //  {
    //    var i1 = i;
    //    questions[i].RecommendedToUsers = await _db.RecommendedQuestionEntities.Where(x => x.UserFromId == userId && x.QuestionId == questions[i1].QuestionId)
    //      .Join(_db.UserEntities, x => x.UserToId, y => y.UserId, (x, y) => y)
    //      .Select(x => new RecommendedToUserDto { UserId = x.UserId, Login = x.Login }).ToListAsync();
    //  }
    //  return questions;

      // var questions =  await _db.RecommendedQuestionEntities.Where(x => x.UserFromId == userId).Join(_db.QuestionEntities,
      //          x => x.QuestionId, y => y.QuestionId,
      //          (x, y) =>new RecommendedQuestionDto

      //          {
      //            QuestionId = y.QuestionId,
      //            UserId = y.UserId,
      //            Condition = y.Condition,
      //            IsAnonymity = y.IsAnonymity,
      //            IsAudience = y.IsAudience,
      //            QuestionType = y.QuestionType,
      //            QuestionAddDate = y.QuestionAddDate,
      //            BackgroundImageLink = y.BackgroundImageLink,
      //            Answers = y.Answers,
      //            Likes = y.Likes,
      //            Dislikes = y.Dislikes,
      //            IsDeleted = y.IsDeleted
      //          }

      // ).OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync() ;

      //// var questionUIds = _db.QuestionIdQueries.Intersect(questions.Select(x => new QuestionIdQuery { QuestionId = x.QuestionId}));// = questions.Select(x => new RecommendedUserQuestionQuery {QuestionId = x.QuestionId, UserId = userId}).ToList();

      // for (var i = 0; i < questions.Count; i++)
      // {
      //   var i1 = i;
      //   questions[i].RecommendedToUsers = await _db.RecommendedQuestionEntities.Where(x => x.UserFromId == userId && x.QuestionId == questions[i1].QuestionId)
      //     .Join(_db.UserEntities, x => x.UserToId, y => y.UserId, (x, y) => y)
      //     .Select(x => new RecommendedToUserDto {UserId = x.UserId, Login = x.Login}).ToListAsync();
      // }
      // return questions;
      //var questionUIds.Join(_db.RecommendedQuestionEntities, x => x.QuestionId, y => y.QuestionId, (x, y) => y)
      //  .Where(x => x.UserFromId == userId).Join(_db.UserEntities, x => x.UserToId, y => y.UserId, (x, y) => y)
      //  .Select(x => new RecommendedToUserDto {UserId = x.UserId, Login = x.Login}).ToListAsync();

      //_db.RecommendedQuestionEntities.Where(x => x.UserFromId == userId).Join(_db.QuestionEntities,
      //  x => x.QuestionId, y => y.QuestionId,
      //  (x, y) => y
      //  //{
      //  //  QuestionId = y.QuestionId,
      //  //  UserId = y.UserId,
      //  //  Condition = y.Condition,
      //  //  IsAnonymity = y.IsAnonymity,
      //  //  IsAudience = y.IsAudience,
      //  //  QuestionType = y.QuestionType,
      //  //  QuestionAddDate = y.QuestionAddDate,
      //  //  BackgroundImageLink = y.BackgroundImageLink,
      //  //  Answers = y.Answers,
      //  //  Likes = y.Likes,
      //  //  Dislikes = y.Dislikes,
      //  //  IsDeleted = y.IsDeleted
      //  //}
      //).OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
 //   }

    public async Task<List<SelectedQuestionDb>> GetSelectedQuestions(int userId, int pageUserId, int offset, int count)
    {
      return await _db.SelectedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNotAnsweredQuestions({userId})")
        .OrderByDescending(x => (5 * x.Likes - 3 * x.Dislikes > 10
                                  ? 5 * x.Likes - 3 * x.Dislikes
                                  : 15 + 5 * x.Likes - 3 * x.Dislikes) - (x.FirstAnswers + x.SecondAnswers))
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<LikedQuestionDb>> GetLikedQuestions(int userId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate)
    {
      return await _db.LikedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getUserLikedQuestions({userId})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<SavedQuestionDb>> GetSavedQuestions(int userId, int offset, int count,
      Expression<Func<SavedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.SavedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {userId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }
  }
}