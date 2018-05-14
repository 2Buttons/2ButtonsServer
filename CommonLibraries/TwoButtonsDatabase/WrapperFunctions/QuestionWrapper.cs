using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.UserQuestions;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class QuestionWrapper
  {
    public static bool TryGetQuestion(TwoButtonsContext db, int userId, int questionId, out QuestionDb question)
    {
      try
      {
        question = db.QuestionDb
                     .FromSql($"select * from dbo.getQuestion({userId}, {questionId})").FirstOrDefault() ??
                   new QuestionDb();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      question = new QuestionDb();
      return false;
    }

    public static bool TryAddQuestion(TwoButtonsContext db, int userId, string condition, string backgroundImageLink,
      int anonymity, int audience, QuestionType questionType, string firstOption, string secondOption, out int questionId)
    {
      var questionAddDate = DateTime.UtcNow;

      var questionIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };

      try
      {
        db.Database.ExecuteSqlCommand(
          $"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
        questionId = (int) questionIdDb.Value;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      questionId = -1;
      return false;
    }

    public static bool TryDeleteQuestion(TwoButtonsContext db, int questionId)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"deleteQuestion {questionId}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateQuestionFeedback(TwoButtonsContext db, int userId, int questionId, FeedbackType feedback)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"updateQuestionFeedback {userId}, {questionId}, {feedback}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateSaved(TwoButtonsContext db, int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      try
      {
        db.Database.ExecuteSqlCommand($"updateFavorites {userId}, {questionId}, {1}, {isInFavorites}, {added}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateFavorites(TwoButtonsContext db, int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      try
      {
        db.Database.ExecuteSqlCommand($"updateFavorites {userId}, {questionId}, {0}, {isInFavorites}, {added}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateAnswer(TwoButtonsContext db, int userId, int questionId, AnswerType answer)
    {
      var answered = DateTime.Now;

      try
      {
        db.Database.ExecuteSqlCommand($"updateAnswer {userId}, {questionId}, {answer}, {answered}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateQuestionBackgroundLink(TwoButtonsContext db, int questionId, string backgroundImageLink)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"updateQuestionBackground {questionId}, {backgroundImageLink}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int getUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate, out IEnumerable<AskedQuestionDb> askedQuestions)
    {
      var isAnonimus = 1;
      

      try
      {
        askedQuestions = db.AskedQuestionsDb
          .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      askedQuestions = new List<AskedQuestionDb>();
      return false;
    }

    public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int getUserId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate, out IEnumerable<LikedQuestionDb> likedQuestions)
    {
      var isAnonimus = 1;
      

      try
      {
        likedQuestions = db.LikedQuestionsDb
          .FromSql($"select * from dbo.getUserLikedQuestions({userId}, {getUserId},   {isAnonimus})")
          .OrderBy(predicate).Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      likedQuestions = new List<LikedQuestionDb>();
      return false;
    }
  }
}