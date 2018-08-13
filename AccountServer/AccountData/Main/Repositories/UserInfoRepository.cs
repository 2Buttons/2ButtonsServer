using System.Linq;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountData.Main.Entities;
using AccountData.Main.Queries;
using CommonLibraries.Exceptions.ApiExceptions;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main.Repositories
{
  public class UserInfoRepository
  {
    private readonly TwoButtonsContext _db;

    public UserInfoRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<string> GetUserAvatar(int userId)
    {
      var user = await _db.UserInfoEntities.FirstOrDefaultAsync(x => x.UserId == userId);
      return user?.OriginalAvatarUrl;
    }

    public async Task<UserInfoDto> FindUserInfoAsync(int userId, int userPageId)
    {
      var user = await _db.UserInfoEntities.Where(x => x.UserId == userPageId).Join(_db.StatisticsEntities,
        x => x.UserId, y => y.UserId, (x, y) => new { UserInfo = x, Statistic = y }).FirstOrDefaultAsync();

      var result = new UserInfoDto
      {
        UserId = userPageId,
        Login = user.UserInfo.Login,
        BirthDate = user.UserInfo.BirthDate,
        SexType = user.UserInfo.SexType,
        City = (await _db.CityEntities.FirstOrDefaultAsync(x => x.CityId == user.UserInfo.CityId)).Name,
        Description = user.UserInfo.Description,
        OriginalAvatarUrl = user.UserInfo.OriginalAvatarUrl,

        AskedQuestionsCount = user.Statistic.AskedQuestions,
        AnswersCount = user.Statistic.AnsweredQuestions,
        FollowersCount = user.Statistic.Followers,
        FollowingsCount = user.Statistic.Followings,
        FavoritesCount = user.Statistic.FavoriteQuestions,
        CommentsCount = user.Statistic.CommentsWritten,

        IsHeFollowed = false,
        IsYouFollowed = false
      };

      if (userId == userPageId) return result;
      {
        await UpdateVisitsAsync(userId, userPageId);
        result.IsHeFollowed =
          _db.UserRelationshipEntities.Any(x => x.UserId == userId && x.StaredUserId == userPageId && x.IsFollowing);
        result.IsYouFollowed =
          _db.UserRelationshipEntities.Any(x => x.UserId == userPageId && x.StaredUserId == userId && x.IsFollowing);
      }
      return result;
    }

    //public async Task<UserInfoDb> FindUserInfoAsync(int userId, int userPageId)
    //{
    //  var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
    //    .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found");
    //  if (userId != userPageId && user.IsYouFollowed) await UpdateVisitsAsync(userId, userPageId);
    //  return user;
    //}

    public async Task<UserInfoEntity> FindUserInfo(int userId)
    {
      return await _db.UserInfoEntities.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<bool> UpdateUserInfoAsync(UserInfoEntity user)
    {
      var oldUserData = await _db.UserInfoEntities.FirstOrDefaultAsync(x => x.UserId == user.UserId);

      oldUserData.Login = user.Login;
      oldUserData.BirthDate = user.BirthDate;
      oldUserData.SexType = user.SexType;
      oldUserData.Description = user.Description;
      oldUserData.OriginalAvatarUrl = user.OriginalAvatarUrl;
      oldUserData.CityId = user.CityId;

      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUserOriginalAvatar(int userId, string originalAvatarUrl)
    {
      var user = _db.UserInfoEntities.FirstOrDefault(x => x.UserId == userId);
      if (user == null) return false;
      user.OriginalAvatarUrl = originalAvatarUrl;
      return await _db.SaveChangesAsync() > 0;
    }


    public async Task<UserStatisticsDb> GetUserStatisticsAsync(int userId)
    {
      return await _db.UserStatisticsDb.FromSql($"select * from dbo.getUserStatistics({userId})")
               .FirstOrDefaultAsync() ?? new UserStatisticsDb();
    }

    public async Task<bool> UpdateVisitsAsync(int userId, int getUserId)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateVisits {userId}, {getUserId}") > 0;
    }
  }
}