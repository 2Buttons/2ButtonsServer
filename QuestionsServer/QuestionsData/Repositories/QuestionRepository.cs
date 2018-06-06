using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO;
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

    public async Task<QuestionDb> FindQuestion(int userId, int questionId)
    {
      var result =  await _db.QuestionDb.AsNoTracking().FromSql($"select * from dbo.getQuestion({userId}, {questionId})")
               .FirstOrDefaultAsync();
      if (result == null) throw new NotFoundException("We do not have this question");
      return result;
    }

    public async Task<QuestionStatisticsDto> GetQuestionFilteredStatistis( int questionId, int minAge, int maxAge, SexType sexType, string city)
    {


      Func<string, int, bool> cityPredicate = (userCityName, filterCityId) =>
      {
        if (userCityName.IsNullOrEmpty()) return true;
        
        var cityId = (_db.CityEntities.FirstOrDefault(x => x.Name == userCityName))?.CityId ?? -1;
        return cityId == filterCityId;
      };

      int cityTId =  city.IsNullOrEmpty()? -1 : (_db.CityEntities.FirstOrDefault(x => x.Name == city))?.CityId ?? -1;

      Func<int, SexType, bool> sexPredicate = (userSex, filterSex) =>
      {
        if (filterSex == SexType.Both) return true;

        return userSex == (int)filterSex;
      };

      Func<DateTime, DateTime, DateTime, bool> agePredicate = (userAge, minAgeUser, maxAgeUser) =>
      {
       // if (minAgeUser == 0 && maxAgeUser == 100) return true;

        return userAge >= minAgeUser && userAge <= maxAgeUser;
      };
      var minDate = minAge.WhenBorned();
      var maxDate = maxAge.WhenBorned();
      Expression<Func<UserEntity, bool>> predicate =
        x => x.BirthDate <= minDate && x.BirthDate >= maxDate && (sexType == SexType.Both || x.SexType == sexType) &&
             (cityTId <= 0 || x.CityId == cityTId);

      if(!_db.QuestionEntities.Any(x=>x.QuestionId == questionId)) throw new NotFoundException("We do not have this question");

      var voters =  await _db.AnswerEntities.Where(x => x.QuestionId == questionId)
        .Join(_db.UserEntities.Where(predicate), a => a.UserId, u => u.UserId, (a, u) => a).GroupBy(x => x.AnswerType)
        .Select(x => new {x.Key, Amount = x.Count()}).ToListAsync();

      var votersList = new List<int>
      {
        voters.FirstOrDefault(x => x.Key == AnswerType.First)?.Amount ?? 0,
        voters.FirstOrDefault(x => x.Key == AnswerType.Second)?.Amount ?? 0
      };


      return  new QuestionStatisticsDto {Voters = votersList};
    }

    public async Task<int> GetQuestionByCommentId(int commentId)
    {
      return (await _db.QuestionIdDb.AsNoTracking().FromSql($"select * from dbo.getQuestionIDFromCommentID({commentId})")
               .FirstOrDefaultAsync())?.QuestionId ?? -1;
    }

    public async Task<int> AddQuestion(int userId, string condition, string backgroundImageLink, int anonymity,
      int audience, QuestionType questionType, string firstOption, string secondOption)
    {
      var questionAddDate = DateTime.UtcNow;

      var questionIdDb = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};

      await _db.Database.ExecuteSqlCommandAsync(
        $"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
      return (int) questionIdDb.Value;
    }

    public async Task<bool> DeleteQuestion(int questionId)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"deleteQuestion {questionId}") > 0;
    }

    public async Task<bool> UpdateQuestionFeedback(int userId, int questionId, FeedbackType feedback)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateQuestionFeedback {userId}, {questionId}, {feedback}") >
             0;
    }

    public async Task<bool> UpdateSaved(int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"updateFavorites {userId}, {questionId}, {1}, {isInFavorites}, {added}") > 0;
    }

    public async Task<bool> UpdateFavorites(int userId, int questionId, bool isInFavorites)
    {
      var added = DateTime.Now;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"updateFavorites {userId}, {questionId}, {0}, {isInFavorites}, {added}") > 0;
    }

    public async Task<bool> UpdateAnswer(int userId, int questionId, AnswerType answerType)
    {
      var answered = DateTime.Now;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }

    public async Task<bool> UpdateQuestionBackgroundLink(int questionId, string backgroundImageLink)
    {
      return await _db.Database.ExecuteSqlCommandAsync(
               $"updateQuestionBackground {questionId}, {backgroundImageLink}") > 0;
    }

    public async Task<List<PhotoDb>> GetPhotos(int userId, int questionId, int answer, int count, DateTime bornAfter,
      DateTime bornBefore, int sex, string city)
    {
      return  _db.PhotoDb.AsNoTracking()
               .FromSql(
                 $"select * from dbo.getPhotos({userId}, {questionId}, {answer}, {count}, {bornAfter}, {bornBefore},  {sex}, {city})")
               .ToList();
    }

    public async Task<List<AnsweredListDb>> GetVoters(int userId, int questionId, int offset, int count,
      AnswerType answerType, DateTime bornAfter, DateTime bornBefore, SexType sexType, string searchedLogin)
    {
      return await _db.AnsweredListDb.AsNoTracking()
               .FromSql(
                 $"select * from dbo.getAnsweredList({userId}, {questionId},   {answerType}, {bornAfter}, {bornBefore}, {sexType}, {searchedLogin})")
               .Skip(offset).Take(count).ToListAsync() ?? new List<AnsweredListDb>();
    }
  }
}