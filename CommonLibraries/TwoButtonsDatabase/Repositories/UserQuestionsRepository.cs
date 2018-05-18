using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.UserQuestions;

namespace TwoButtonsDatabase.Repositories
{
  public class UserQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public UserQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public bool TryAddRecommendedQuestion(int userToId, int pageUserId, int questionId)
    {
      var recommendedDate = DateTime.UtcNow;
      try
      {
        _db.Database.ExecuteSqlCommand($"addTag {userToId}, {pageUserId}, {questionId}, {recommendedDate}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryGetUserAskedQuestions(int userId, int pageUserId, int offset, int count,
      Expression<Func<UserAskedQuestionDb, object>> predicate, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 0;

      try
      {
        userAskedQuestions = _db.UserAskedQuestionsDb
          .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userAskedQuestions = new List<UserAskedQuestionDb>();
      return false;
    }

    public bool TryGetUserAnsweredQuestions(int userId, int pageUserId, int offset,
      int count, out IEnumerable<UserAnsweredQuestionDb> userAnsweredQuestions)
    {
      var isAnonimus = userId == pageUserId ? 1 : 0;


      try
      {
        userAnsweredQuestions = _db.UserAnsweredQuestionsDb
          .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {isAnonimus})")
          .OrderBy(x => x.QuestionAddDate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userAnsweredQuestions = new List<UserAnsweredQuestionDb>();
      return false;
    }

    public bool TryGetUserFavoriteQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserFavoriteQuestionDb, object>> predicate,
      out IEnumerable<UserFavoriteQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 0;

      try
      {
        userFavoriteQuestions = _db.UserFavoriteQuestionsDb
          .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userFavoriteQuestions = new List<UserFavoriteQuestionDb>();
      return false;
    }

    public bool TryGetUserCommentedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserCommentedQuestionDb, object>> predicate,
      out IEnumerable<UserCommentedQuestionDb> userCommentedQuestions)
    {
      try
      {
        userCommentedQuestions = _db.UserCommentedQuestionsDb
          .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userCommentedQuestions = new List<UserCommentedQuestionDb>();
      return false;
    }

    public bool TryGeTopQuestions(int userId, bool isOnlyNew, int offset, int count,
      DateTime topAfterDate, Expression<Func<TopQuestionDb, object>> predicate,
      out IEnumerable<TopQuestionDb> topQuestions)
    {
      try
      {
        topQuestions = _db.TopQuestionsDb
          .FromSql($"select * from dbo.getTop({userId}, {topAfterDate}, {isOnlyNew})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      topQuestions = new List<TopQuestionDb>();
      return false;
    }

   
    
    public bool TryGetAskedQuestions(int userId, int pageUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate, out IEnumerable<AskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 1;


      try
      {
        userAskedQuestions = _db.AskedQuestionsDb
          .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId},   {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userAskedQuestions = new List<AskedQuestionDb>();
      return false;
    }

    public bool TryGetRecommendedQuestions(int userId, int pageUserId, int offset,
      int count,
      Expression<Func<RecommendedQuestionDb, object>> predicate,
      out IEnumerable<RecommendedQuestionDb> recommendedQuestions)
    {
      try
      {
        recommendedQuestions = _db.RecommendedQuestionsDb
          .FromSql($"select * from dbo.[getUserRecommendedQuestions]({userId})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedQuestions = new List<RecommendedQuestionDb>();
      return false;
    }

    public bool TryGetLikedQuestions(int userId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate, out IEnumerable<LikedQuestionDb> userAnsweredQuestions)
    {
      try
      {
        userAnsweredQuestions = _db.LikedQuestionsDb
          .FromSql($"select * from dbo.getUserLikedQuestions({userId})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userAnsweredQuestions = new List<LikedQuestionDb>();
      return false;
    }

    public bool TryGetSavedQuestions(int userId, int offset, int count,
      Expression<Func<SavedQuestionDb, object>> predicate, out IEnumerable<SavedQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 1;


      try
      {
        userFavoriteQuestions = _db.SavedQuestionsDb
          .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {userId}, {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userFavoriteQuestions = new List<SavedQuestionDb>();
      return false;
    }
  }
}