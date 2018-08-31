using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Entities.Main;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Extensions;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO;
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

    public async Task<EmptyQuestionDto> GetEmptyQuestion(int questionId)
    {
      var question = await _db.QuestionEntities.Where(x => x.QuestionId == questionId).Join(_db.UserInfoEntities, x=>x.UserId, y=>y.UserId,(x,y)=>new {Q = x, U = y}).FirstOrDefaultAsync();
      if (question == null) throw new NotFoundException("We do not have this question");
      var oprion1 =
        await  _db.OptionEntities.FirstOrDefaultAsync(x => x.QuestionId == questionId && x.Position == 1);
      var oprion2 =
        await _db.OptionEntities.FirstOrDefaultAsync(x => x.QuestionId == questionId && x.Position == 2);
      var result = new EmptyQuestionDto
      {
        QuestionId = question.Q.QuestionId,
        Author =
          new AuthorDto
          {
            UserId = question.Q.UserId,
            FirstName = question.U.FirstName,
            LastName= question.U.LastName,
            SexType = question.U.SexType,
            OriginalAvatarUrl = question.U.OriginalAvatarUrl
          },
        Condition = question.Q.Condition,
        Options =
          new List<OptionDto>
          {
            new OptionDto {Text = oprion1.Text, Voters = oprion1.AnswersCount},
            new OptionDto {Text = oprion2.Text, Voters = oprion1.AnswersCount}
          },
        QuestionType = question.Q.QuestionType,
        OriginalBackgroundUrl = question.Q.OriginalBackgroundUrl,
        CommentsCount = question.Q.CommentsCount,
        DislikesCount = question.Q.DislikesCount,
        LikesCount = question.Q.LikesCount,
        AddedDate = question.Q.AddedDate
      };
      return result;
    }

    public async Task<string> GetBackground(int questionId)
    {
      var result = await _db.QuestionEntities.FirstOrDefaultAsync(x => x.QuestionId == questionId);
      return result?.OriginalBackgroundUrl;
    }

    public async Task<List<QuestionStatisticDto>> GetQuestionStatistic(int userId, int questionId, int minAge, int maxAge,
      SexType sexType, string city)
    {
     
      var cityId = city.IsNullOrEmpty() ? -1 : _db.CityEntities.FirstOrDefault(x => x.Name == city)?.CityId ?? -1;

   
      var minDate = minAge.WhenBorned();
      var maxDate = maxAge.WhenBorned();
      Expression<Func<Tuple<UserInfoEntity, AnswerEntity>, bool>> predicate =
        x => x.Item1.BirthDate <= minDate && x.Item1.BirthDate >= maxDate &&
             (sexType == SexType.Both || x.Item1.SexType == sexType) && (cityId <= 0 || x.Item1.CityId == cityId);

      if (!_db.QuestionEntities.Any(x => x.QuestionId == questionId))
        throw new NotFoundException("We do not have this question");

      var questions = _db.AnswerEntities.Where(x => x.QuestionId == questionId).Join(_db.UserInfoEntities, a => a.UserId,
        u => u.UserId, (a, u) => new Tuple<UserInfoEntity, AnswerEntity>(u, a)).Where(predicate);
      var votersCount = await questions.GroupBy(x=>x.Item2.AnswerType).Select(x=> new {Type = x.Key, Count = x.Count()}).ToListAsync();

      var friendIds = _db.FollowingEntities.Where(x => x.UserId == userId )
        .Join(_db.FollowingEntities, x => x.FollowingId, y => y.UserId,
          (x, y) => new {UserId = x.UserId, FollowingId = x.FollowingId, FollowingToMeId = y.FollowingId })
        .Where(x=>x.UserId == x.FollowingToMeId).Select(x => x.FollowingId)
        .ToList();

      var votersFriends =  friendIds.Join(questions, a=>a, b=>b.Item1.UserId, (a,b)=> b).ToList();//.Where(x=>x.Follow.(x => x.Item2.AnswerType).Select(x => new { Type = x.Key, Count = x.Count() }).ToListAsync();
      var friendsFirstAnswer = votersFriends.Where(x => x.Item2.AnswerType == AnswerType.First).Take(5).Select(x=>new VoterFriendDto{UserId = x.Item2.UserId, Age = x.Item1.BirthDate.Age(), FirstName = x.Item1.FirstName, LastName = x.Item1.LastName, SexType = x.Item1.SexType, OriginalAvatarUrl = x.Item1.OriginalAvatarUrl }).ToList();
      var friendsSecondAnswer = votersFriends.Where(x => x.Item2.AnswerType == AnswerType.Second).Take(5).Select(x => new VoterFriendDto { UserId = x.Item2.UserId, Age = x.Item1.BirthDate.Age(), FirstName = x.Item1.FirstName, LastName = x.Item1.LastName, SexType = x.Item1.SexType, OriginalAvatarUrl = x.Item1.OriginalAvatarUrl }).ToList();
      //var countFirstAnswerType = voters.Count(x => x.Item2.AnswerType == AnswerType.First);
      //var countSecondAnswerType = voters.Count - countFirstAnswerType;

      //var votersList = new List<int> {countFirstAnswerType, countSecondAnswerType};
     // var voresList = new List<int> {votersCount.FirstOrDefault(x => x.Type == AnswerType.First)?.Count ?? 0, votersCount.FirstOrDefault(x => x.Type == AnswerType.Second)?.Count ?? 0 };
      return new List<QuestionStatisticDto>
      {
        new QuestionStatisticDto
        {
          Count = votersCount.FirstOrDefault(x => x.Type == AnswerType.First)?.Count ?? 0,
          Friends = friendsFirstAnswer
        },
        new QuestionStatisticDto
        {
          Count = votersCount.FirstOrDefault(x => x.Type == AnswerType.Second)?.Count ?? 0,
          Friends = friendsSecondAnswer
        },
      };
    }

    public async Task<List<string>> GetCustomQuestionBackgrounds(int userId)
    {
      return await _db.QuestionEntities.Where(x => x.UserId == userId).Select(x => x.OriginalBackgroundUrl).Distinct().ToListAsync();
    }

    public async Task<QiestionStatisticUsersDto> GetQuestionStatistiсUsers(int userId, int questionId, int minAge, int maxAge,
      SexType sexType, string city, int offset, int count)
    {
      var cityId = city.IsNullOrEmpty() ? -1 : _db.CityEntities.FirstOrDefault(x => x.Name == city)?.CityId ?? -1;

      var minDate = minAge.WhenBorned();
      var maxDate = maxAge.WhenBorned();

      Expression<Func<Tuple<UserInfoEntity, AnswerEntity>, bool>> predicate =
        x => x.Item1.BirthDate <= minDate && x.Item1.BirthDate >= maxDate &&
             (sexType == SexType.Both || x.Item1.SexType == sexType) && (cityId <= 0 || x.Item1.CityId == cityId);

      if (!_db.QuestionEntities.Any(x => x.QuestionId == questionId))
        throw new NotFoundException("We do not have this question");

      var voters = await _db.AnswerEntities.Where(x => x.QuestionId == questionId)
        .Join(_db.UserInfoEntities, a => a.UserId, u => u.UserId, (a, u) => new Tuple<UserInfoEntity, AnswerEntity>(u, a))
        .Where(predicate)
        .Join(_db.CityEntities, uc => uc.Item1.CityId, c => c.CityId, (f, s) => new {f, s, isYouFollowed = _db.FollowingEntities.Any(x=>x.UserId== userId && x.FollowingId == f.Item1.UserId), isHeFollowed  = _db.FollowingEntities.Any(x => x.UserId == f.Item1.UserId && x.FollowingId == userId) })
        .OrderByDescending(x=>x.f.Item2.AnsweredDate).Skip(offset).Take(count).ToListAsync();

      var firstUsers = new List<VoterUserDto>();
      var secondUsers = new List<VoterUserDto>();
      foreach (var voter in voters)
      {
        var user = new VoterUserDto
        {
          UserId = voter.f.Item1.UserId,
          Age = voter.f.Item1.BirthDate.Age(),
          IsHeFollowed = voter.isHeFollowed,
          IsYouFollowed =voter.isYouFollowed,
          City = voter.s.Name,
          FirstName = voter.f.Item1.FirstName,
          LastName = voter.f.Item1.LastName,
          SexType = voter.f.Item1.SexType,

          OriginalAvatarUrl = voter.f.Item1.OriginalAvatarUrl
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

      return new QiestionStatisticUsersDto {Voters = new List<List<VoterUserDto>> {firstUsers, secondUsers}};
    }

    public async Task<int> GetQuestionByCommentId(int commentId)
    {
      return (await _db.QuestionIdDb.AsNoTracking()
               .FromSql($"select * from dbo.getQuestionIDFromCommentID({commentId})").FirstOrDefaultAsync())
             ?.QuestionId ?? -1;
    }

    public async Task<int> AddQuestion(int userId, string condition, string originalBackgroundUrl,
      AudienceType audienceType, QuestionType questionType, bool isAnonymous, string firstOption, string secondOption)
    {
      var questionAddDate = DateTime.UtcNow;

      var questionIdDb = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};
      var isAnonymousInt = isAnonymous ? 1 : 0;

      await _db.Database.ExecuteSqlCommandAsync(
        $"addQuestion {userId}, {condition}, {originalBackgroundUrl}, {audienceType}, {questionType}, {isAnonymousInt}, {firstOption}, {secondOption}, {questionAddDate}, {questionIdDb} OUT");
      return (int) questionIdDb.Value;
    }

    public async Task<bool> DeleteQuestion(int questionId)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"deleteQuestion {questionId}") > 0;
    }


    public async Task<bool> UpdateQuestionFeedback(int userId, int questionId, QuestionFeedbackType feedback)
    {
      var answer = await _db.AnswerEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId);
      if (answer == null)
      {
        answer = new AnswerEntity
        {
          UserId = userId,
          QuestionId = questionId,
          FeedbackType =  feedback,
          IsDeleted = false,
          AnswerType = AnswerType.NoAnswer,
          AnsweredDate = DateTime.UtcNow,
          IsFavorite = false,
          IsSaved = false
        };
        _db.AnswerEntities.Add(answer);
        return await _db.SaveChangesAsync() > 0;
      }

      answer.FeedbackType = feedback;
      return await _db.SaveChangesAsync() > 0;
      //return await _db.Database.ExecuteSqlCommandAsync($"updateQuestionFeedback {userId}, {questionId}, {feedback}") >
      //       0;
    }

    public async Task<bool> UpdateSaved(int userId, int questionId, bool isSaved)
    {

      var answer = await _db.AnswerEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId);
      if (answer == null)
      {
        answer = new AnswerEntity
        {
          UserId = userId,
          QuestionId = questionId,
          FeedbackType =  QuestionFeedbackType.Neutral,
          IsDeleted = false,
          AnswerType = AnswerType.NoAnswer,
          AnsweredDate = DateTime.UtcNow,
          IsFavorite = false,
          IsSaved = isSaved
        };
        _db.AnswerEntities.Add(answer);
        
      }
      var stat = await _db.StatisticsEntities.FirstOrDefaultAsync(x => x.UserId == userId);
      if (isSaved)
        stat.FavoriteQuestions++;
      else
        stat.FavoriteQuestions--;

      answer.IsSaved = isSaved;
      return await _db.SaveChangesAsync() > 0;

      //var added = DateTime.Now;

      //return await _db.Database.ExecuteSqlCommandAsync(
      //         $"updateFavorites {userId}, {questionId}, {1}, {isFavorite}, {added}") > 0;
    }

    public async Task<bool> UpdateFavorites(int userId, int questionId, bool isFavorite)
    {
      var answer = await _db.AnswerEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId);
      if (answer == null)
      {
        answer = new AnswerEntity
        {
          UserId = userId,
          QuestionId = questionId,
          FeedbackType = QuestionFeedbackType.Neutral,
          IsDeleted = false,
          AnswerType = AnswerType.NoAnswer,
          AnsweredDate = DateTime.UtcNow,
          IsFavorite = isFavorite,
          IsSaved = false
        };
        _db.AnswerEntities.Add(answer);
       
      }
      var stat = await _db.StatisticsEntities.FirstOrDefaultAsync(x => x.UserId == userId);
      if (isFavorite)
        stat.PublicFavoriteQuestions++;
      else
        stat.PublicFavoriteQuestions--;

      answer.IsFavorite = isFavorite;
      return await _db.SaveChangesAsync() > 0;

      //var added = DateTime.Now;

      //return await _db.Database.ExecuteSqlCommandAsync(
      //         $"updateFavorites {userId}, {questionId}, {0}, {isFavorite}, {added}") > 0;
    }

    public async Task<bool> UpdateAnswer(int userId, int questionId, AnswerType answerType)
    {

      var answer =  _db.AnswerEntities.FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId);
      if (answer == null)
      {
        answer = new AnswerEntity
        {
          UserId = userId,
          QuestionId = questionId,
          FeedbackType = QuestionFeedbackType.Neutral,
          IsDeleted = false,
          AnswerType = answerType,
          AnsweredDate = DateTime.UtcNow,
          IsFavorite = false,
          IsSaved = false
        };
        _db.AnswerEntities.Add(answer);
      }

      answer.AnswerType = answerType;
     // _db.AnswerEntities.Update(answer);
      return  _db.SaveChanges() > 0;
      //var answered = DateTime.Now;

      //return await _db.Database.ExecuteSqlCommandAsync(
      //         $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }

    public async Task<bool> UpdateQuestionBackgroundUrl(int questionId, string backgroundImageUrl)
    {
      return await _db.Database.ExecuteSqlCommandAsync(
               $"updateQuestionBackground {questionId}, {backgroundImageUrl}") > 0;
    }

    public async Task<List<PhotoDb>> GetPhotos(int userId, int questionId, int answer, int count, DateTime bornAfter,
      DateTime bornBefore, int sex, string city)
    {
      return _db.PhotoDb.AsNoTracking()
        .FromSql(
          $"select * from dbo.[getPhotos]({userId}, {questionId}, {answer}, {count}, {bornAfter}, {bornBefore},  {sex}, {city})")
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

    public async Task<QuestionDb> SearchSimilarQuestion(int questionId)
    {
      var questionPattern = await _db.QuestionEntities.FirstOrDefaultAsync(x => x.QuestionId == questionId);
      if (questionPattern == null) return null;
      return null;
      //var searchedWords = questionPattern.Condition.Trim().ToLower().Split(' ').Select();
      //_db.QuestionEntities.Where(x=>x.Condition.ToLower().)

      // ;.AsNoTracking().FromSql($"select * from dbo.getQuestion({userId}, {questionId})")
      //  .FirstOrDefaultAsync();
      //if (result == null) throw new NotFoundException("We do not have this question");
      //return result;
    }
  }
}