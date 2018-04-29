using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
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



    public static bool TryGetUserAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 0;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;

      try
      {
        userAskedQuestions = db.UserAskedQuestionsDb
            .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

    public static bool TryGetUserAnsweredQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount, out IEnumerable<UserAnsweredQuestionDb> userAnsweredQuestions)
    {
      var isAnonimus = 0;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userAnsweredQuestions = db.UserAnsweredQuestionsDb
            .FromSql(
                $"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

    public static bool TryGetUserFavoriteQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount, out IEnumerable<UserFavoriteQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 0;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userFavoriteQuestions = db.UserFavoriteQuestionsDb
            .FromSql(
                $"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

    public static bool TryGetUserCommentedQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount,
        out IEnumerable<UserCommentedQuestionDb> userCommentedQuestions)
    {
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userCommentedQuestions = db.UserCommentedQuestionsDb
            .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId}, {fromLine}, {toLine})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userCommentedQuestions = new List<UserCommentedQuestionDb>();
      return false;
    }

    public static bool TryGeTopQuestions(TwoButtonsContext db, int userId, bool isOnlyNew, int page, int amount, DateTime topAfterDate,
        out IEnumerable<TopQuestionDb> topQuestions)
    {
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        topQuestions = db.TopQuestionsDb
            .FromSql($"select * from dbo.getTop({userId}, {fromLine}, {toLine}, {topAfterDate}, {isOnlyNew})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      topQuestions = new List<TopQuestionDb>();
      return false;
    }



    public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount, out IEnumerable<AskedQuestionDb> userAskedQuestions)
    {
      var isAnonimus = 1;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userAskedQuestions = db.AskedQuestionsDb
            .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

    public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int pageUserId, int page, int amount, out IEnumerable<LikedQuestionDb> userAnsweredQuestions)
    {
      var isAnonimus = 1;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userAnsweredQuestions = db.LikedQuestionsDb
            .FromSql(
                $"select * from dbo.getUserLikedQuestions({userId}, {pageUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

    public static bool TryGetSavedQuestions(TwoButtonsContext db, int userId, int page, int amount, out IEnumerable<SavedQuestionDb> userFavoriteQuestions)
    {
      var isAnonimus = 1;
      int fromLine = page * amount - amount + 1;
      int toLine = page * amount;
      try
      {
        userFavoriteQuestions = db.SavedQuestionsDb
            .FromSql(
                $"select * from dbo.getUserFavoriteQuestions({userId}, {fromLine}, {toLine}, {isAnonimus})")
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