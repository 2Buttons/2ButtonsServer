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

namespace TwoButtonsDatabase.Repositories
{
  public class QuestionRepository
  {
    private readonly TwoButtonsContext _db;

    public QuestionRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public  bool TryGetQuestion( int userId, int questionId, out QuestionDb question)
    {
      try
      {
        question = _db.QuestionDb
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

    public  bool TryAddQuestion( int userId, string condition, string backgroundImageLink,
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
       var rows =  _db.Database.ExecuteSqlCommand(
          $"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
        questionId = (int) questionIdDb.Value;
        return rows >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      questionId = -1;
      return false;
    }

    public  bool TryDeleteQuestion( int questionId,out bool isChanged)
    {
      try
      {

        isChanged =  _db.Database.ExecuteSqlCommand($"deleteQuestion {questionId}") > 0;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      isChanged = false;
      return false;
    }

    public  bool TryUpdateQuestionFeedback( int userId, int questionId, FeedbackType feedback, out bool isChanged)
    {
      try
      {
        isChanged=  _db.Database.ExecuteSqlCommand($"updateQuestionFeedback {userId}, {questionId}, {feedback}") > 0;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      isChanged = false;
      return false;
    }

    public  bool TryUpdateSaved( int userId, int questionId, bool isInFavorites, out bool isChanged)
    {
      var added = DateTime.Now;

      try
      {
        isChanged =  _db.Database.ExecuteSqlCommand($"updateFavorites {userId}, {questionId}, {1}, {isInFavorites}, {added}") > 0;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      isChanged = false;
      return false;
    }

    public  bool TryUpdateFavorites( int userId, int questionId, bool isInFavorites, out bool isChanged)
    {
      var added = DateTime.Now;

      try
      {
        isChanged =  _db.Database.ExecuteSqlCommand($"updateFavorites {userId}, {questionId}, {0}, {isInFavorites}, {added}") >0;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      isChanged = false;
      return false;
    }

    public  bool TryUpdateAnswer( int userId, int questionId, AnswerType answerType, out bool isChanged)
    {
      var answered = DateTime.Now;

      try
      {
        isChanged =  _db.Database.ExecuteSqlCommand($"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      isChanged = false;
      return false;
    }

    public  bool TryUpdateQuestionBackgroundLink( int questionId, string backgroundImageLink)
    {
      try
      {
        return _db.Database.ExecuteSqlCommand($"updateQuestionBackground {questionId}, {backgroundImageLink}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryGetPhotos(int userId, int questionId, int answer, int count,
      DateTime bornAfter, DateTime bornBefore, int sex, string city, out IEnumerable<PhotoDb> photos)
    {
      try
      {
        photos = _db.PhotoDb
          .FromSql(
            $"select * from dbo.getPhotos({userId}, {questionId}, {answer}, {count}, {bornAfter}, {bornBefore},  {sex}, {city})")
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      photos = new List<PhotoDb>();
      return false;
    }

    public bool TryGetVoters(int userId, int questionId, int offset, int count, AnswerType answerType,
      DateTime bornAfter, DateTime bornBefore, SexType sexType, string searchedLogin,
      out IEnumerable<AnsweredListDb> answeredList)
    {
      try
      {
        answeredList = _db.AnsweredListDb
          .FromSql(
            $"select * from dbo.getAnsweredList({userId}, {questionId},   {answerType}, {bornAfter}, {bornBefore}, {sexType}, {searchedLogin})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      answeredList = new List<AnsweredListDb>();
      return false;
    }
  }
}