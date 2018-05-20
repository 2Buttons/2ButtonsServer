using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;

namespace QuestionsData.Repositories
{
  public class QuestionRepository
  {
    private readonly TwoButtonsContext _db;

    public QuestionRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<QuestionDb> GetQuestion(int userId, int questionId)
    {
      try
      {
        return await _db.QuestionDb.AsNoTracking()
                     .FromSql($"select * from dbo.getQuestion({userId}, {questionId})").FirstOrDefaultAsync() ??
                   new QuestionDb();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return  new QuestionDb();
    }

    public async Task<int> AddQuestion(int userId, string condition, string backgroundImageLink,
      int anonymity, int audience, QuestionType questionType, string firstOption, string secondOption)
    {
      var questionAddDate = DateTime.UtcNow;

      var questionIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };

      try
      {
       await _db.Database.ExecuteSqlCommandAsync(
           $"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
        return (int)questionIdDb.Value;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return -1;
    }

    public async Task<bool> DeleteQuestion(int questionId)
    {
      try
      {

        return await _db.Database.ExecuteSqlCommandAsync($"deleteQuestion {questionId}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<bool> UpdateQuestionFeedback(int userId, int questionId, FeedbackType feedback)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateQuestionFeedback {userId}, {questionId}, {feedback}") > 0;

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      return false;
    }

    public async Task<bool> UpdateSaved(int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      try
      {
      return await _db.Database.ExecuteSqlCommandAsync($"updateFavorites {userId}, {questionId}, {1}, {isInFavorites}, {added}") > 0;

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      return false;
    }

    public async Task<bool> UpdateFavorites(int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateFavorites {userId}, {questionId}, {0}, {isInFavorites}, {added}") > 0;

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      return false;
    }

    public async Task<bool> UpdateAnswer(int userId, int questionId, AnswerType answerType)
    {
      var answered = DateTime.Now;

      try
      {
         return await _db.Database.ExecuteSqlCommandAsync($"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      return false;
    }

    public async Task<bool> UpdateQuestionBackgroundLink(int questionId, string backgroundImageLink)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateQuestionBackground {questionId}, {backgroundImageLink}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<List<PhotoDb>> GetPhotos(int userId, int questionId, int answer, int count,
      DateTime bornAfter, DateTime bornBefore, int sex, string city)
    {
      try
      {
        return await _db.PhotoDb.AsNoTracking()
          .FromSql(
            $"select * from dbo.getPhotos({userId}, {questionId}, {answer}, {count}, {bornAfter}, {bornBefore},  {sex}, {city})")
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<PhotoDb>();
    }

    public async Task<List<AnsweredListDb>> GetVoters(int userId, int questionId, int offset, int count, AnswerType answerType,
      DateTime bornAfter, DateTime bornBefore, SexType sexType, string searchedLogin)
    {
      try
      {
        return await _db.AnsweredListDb.AsNoTracking()
          .FromSql(
            $"select * from dbo.getAnsweredList({userId}, {questionId},   {answerType}, {bornAfter}, {bornBefore}, {sexType}, {searchedLogin})")
          .Skip(offset).Take(count)
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return new List<AnsweredListDb>();
      }

    }
  }
}