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
using QuestionsData.Queries;

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
      var result = await _db.QuestionDb.AsNoTracking().FromSql($"select * from dbo.getQuestion({userId}, {questionId})")
        .FirstOrDefaultAsync();
      if (result == null) throw new NotFoundException("We do not have this question");
      return result;
    }

    public async Task<QuestionStatisticDto> GetQuestionStatistic(int questionId, int minAge, int maxAge,
      SexType sexType, string city)
    {
     
      var cityId = city.IsNullOrEmpty() ? -1 : _db.CityEntities.FirstOrDefault(x => x.Name == city)?.CityId ?? -1;

   
      var minDate = minAge.WhenBorned();
      var maxDate = maxAge.WhenBorned();
      Expression<Func<Tuple<UserEntity, AnswerEntity>, bool>> predicate =
        x => x.Item1.BirthDate <= minDate && x.Item1.BirthDate >= maxDate &&
             (sexType == SexType.Both || x.Item1.SexType == sexType) && (cityId <= 0 || x.Item1.CityId == cityId);

      if (!_db.QuestionEntities.Any(x => x.QuestionId == questionId))
        throw new NotFoundException("We do not have this question");

      var voters = await _db.AnswerEntities.Where(x => x.QuestionId == questionId)
        .Join(_db.UserEntities, a => a.UserId, u => u.UserId, (a, u) => new Tuple<UserEntity, AnswerEntity>(u, a))
        .Where(predicate).ToListAsync();

      var countFirstAnswerType = voters.Count(x => x.Item2.AnswerType == AnswerType.First);
      var countSecondAnswerType = voters.Count - countFirstAnswerType;

      var votersList = new List<int> {countFirstAnswerType, countSecondAnswerType};

      return new QuestionStatisticDto {Voters = votersList};
    }

    public async Task<List<string>> GetCustomQuestionBackgrounds(int userId)
    {
      return await _db.QuestionEntities.Where(x => x.UserId == userId).Select(x => x.BackgroundImageLink).ToListAsync();
    }

    public async Task<QiestionStatisticUsersDto> GetQuestionStatistiсUsers(int userId, int questionId, int minAge, int maxAge,
      SexType sexType, string city, int offset, int count)
    {
      var cityId = city.IsNullOrEmpty() ? -1 : _db.CityEntities.FirstOrDefault(x => x.Name == city)?.CityId ?? -1;

      var minDate = minAge.WhenBorned();
      var maxDate = maxAge.WhenBorned();

      Expression<Func<Tuple<UserEntity, AnswerEntity>, bool>> predicate =
        x => x.Item1.BirthDate <= minDate && x.Item1.BirthDate >= maxDate &&
             (sexType == SexType.Both || x.Item1.SexType == sexType) && (cityId <= 0 || x.Item1.CityId == cityId);

      if (!_db.QuestionEntities.Any(x => x.QuestionId == questionId))
        throw new NotFoundException("We do not have this question");

      var voters = await _db.AnswerEntities.Where(x => x.QuestionId == questionId)
        .Join(_db.UserEntities, a => a.UserId, u => u.UserId, (a, u) => new Tuple<UserEntity, AnswerEntity>(u, a))
        .Where(predicate).Join(_db.CityEntities, uc => uc.Item1.CityId, c => c.CityId, (f, s) => new {f, s, isYouFollowed = _db.FollowEntities.Any(x=>x.UserdId== userId && x.FollowToId == f.Item1.UserId), isHeFollowed  = _db.FollowEntities.Any(x => x.UserdId == f.Item1.UserId && x.FollowToId == userId) })
        .OrderByDescending(x=>x.f.Item2.AnswerDate).Skip(offset).Take(count).ToListAsync();

      var firstUsers = new List<VoterDto>();
      var secondUsers = new List<VoterDto>();
      foreach (var voter in voters)
      {
        var user = new VoterDto
        {
          UserId = voter.f.Item1.UserId,
          Age = voter.f.Item1.BirthDate.Age(),
          IsHeFollowed = voter.isHeFollowed,
          IsYouFollowed =voter.isYouFollowed,
          City = voter.s.Name,
          Login = voter.f.Item1.Login,
          Sex = voter.f.Item1.SexType,
          
          SmallAvatarLink = voter.f.Item1.SmallAvatarLink
        };

        switch (voter.f.Item2.AnswerType)
        {
          case AnswerType.First:
            firstUsers.Add(user);
            break;
          case AnswerType.Second:
            secondUsers.Add(user);
            break;
        }
      }

      return new QiestionStatisticUsersDto {Voters = new List<List<VoterDto>> {firstUsers, secondUsers}};
    }

    public async Task<int> GetQuestionByCommentId(int commentId)
    {
      return (await _db.QuestionIdDb.AsNoTracking()
               .FromSql($"select * from dbo.getQuestionIDFromCommentID({commentId})").FirstOrDefaultAsync())
             ?.QuestionId ?? -1;
    }

    public async Task<int> AddQuestion(int userId, string condition, string backgroundImageLink, int anonymity,
      AudienceType audienceType, QuestionType questionType, string firstOption, string secondOption)
    {
      var questionAddDate = DateTime.UtcNow;

      var questionIdDb = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};

      await _db.Database.ExecuteSqlCommandAsync(
        $"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audienceType}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
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
      return _db.PhotoDb.AsNoTracking()
        .FromSql(
          $"select * from dbo.getPhotos({userId}, {questionId}, {answer}, {count}, {bornAfter}, {bornBefore},  {sex}, {city})")
        .ToList();
    }

    public List<PhotoDb> GetPhotos(TwoButtonsContext context, int userId, int questionId, int answer, int count,
      DateTime bornAfter, DateTime bornBefore, int sex, string city)
    {
      return context.PhotoDb.AsNoTracking()
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