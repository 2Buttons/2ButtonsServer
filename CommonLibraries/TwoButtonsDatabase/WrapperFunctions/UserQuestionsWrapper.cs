using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.UserQuestions;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class UserQuestionsWrapper
  {
    public static bool TryAddRecommendedQuestion(TwoButtonsContext db, int userToId, int userFromId, int questionId)
    {
      var recommendedDate = DateTime.UtcNow;
      try
      {
        db.Database.ExecuteSqlCommand($"addTag {userToId}, {userFromId}, {questionId}, {recommendedDate}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }


    public static bool TryGetUserAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset, int count,
      Expression<Func<UserAskedQuestionDb, object>> predicate, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 0;
      
      try
      {
        userAskedQuestions = db.UserAskedQuestionsDb
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

    public static bool TryGetUserAnsweredQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset,
      int count, out IEnumerable<UserAnsweredQuestionDb> userAnsweredQuestions)
    {
      var isAnonimus = userId == pageUserId ? 1 : 0;
      

      try
      {
        userAnsweredQuestions = db.UserAnsweredQuestionsDb
          .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {isAnonimus})")
          .OrderBy(x=>x.QuestionAddDate).Skip(offset).Take(count)
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

    public static bool TryGetUserFavoriteQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset,
      int count, Expression<Func<UserFavoriteQuestionDb, object>> predicate,
      out IEnumerable<UserFavoriteQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 0;
      
      try
      {
        userFavoriteQuestions = db.UserFavoriteQuestionsDb
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

    public static bool TryGetUserCommentedQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset,
      int count, Expression<Func<UserCommentedQuestionDb, object>> predicate,
      out IEnumerable<UserCommentedQuestionDb> userCommentedQuestions)
    {
      

      try
      {
        userCommentedQuestions = db.UserCommentedQuestionsDb
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

    public static bool TryGeTopQuestions(TwoButtonsContext db, int userId, bool isOnlyNew, int offset, int count,
       DateTime topAfterDate,Expression<Func<TopQuestionDb, object>> predicate,
      out IEnumerable<TopQuestionDb> topQuestions)
    {
      

      try
      {
        topQuestions = db.TopQuestionsDb
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

    public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate, out IEnumerable<AskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 1;
      

      try
      {
        userAskedQuestions = db.AskedQuestionsDb
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

    public static bool TryGetRecommendedQuestions(TwoButtonsContext db, int userId, int pageUserId, int offset, int count,
      Expression<Func<RecommendedQuestionDb, object>> predicate, out IEnumerable<RecommendedQuestionDb> recommendedQuestions)
    {
      try
      {
        recommendedQuestions = db.RecommendedQuestionsDb
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

    public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate, out IEnumerable<LikedQuestionDb> userAnsweredQuestions)
    {
      

      try
      {
        userAnsweredQuestions = db.LikedQuestionsDb
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

    public static bool TryGetSavedQuestions(TwoButtonsContext db, int userId, int offset, int count,
      Expression<Func<SavedQuestionDb, object>> predicate, out IEnumerable<SavedQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 1;
      

      try
      {
        userFavoriteQuestions = db.SavedQuestionsDb
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