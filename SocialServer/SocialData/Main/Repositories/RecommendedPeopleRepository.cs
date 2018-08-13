using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialData.Main.DTO;
using SocialData.Main.Entities.Recommended;

namespace SocialData.Main.Repositories
{
  //TODO https://docs.microsoft.com/ru-ru/ef/core/modeling/owned-entities
  public class RecommendedPeopleRepository
  {
    private readonly TwoButtonsContext _db;

    public RecommendedPeopleRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    /// <summary>
    ///   возвращает список пользователей, подписанных на тебя.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<List<RecommendedFromFollowersDto>> GetRecommendedFromFollowers(int userId, int offset, int count)
    {
      return await _db.UserRelationshipEntities.Where(x => x.StaredUserId == userId && x.IsFollowing)
        .Join(_db.UserRelationshipEntities.Where(x => x.UserId == userId).DefaultIfEmpty(), x => x.UserId,
          y => y.StaredUserId, (x, y) => new { X = x, IsHeFollowed = y }).Where(x => !x.IsHeFollowed.IsFollowing)
        .Join(_db.UserInfoEntities, x => x.X.UserId, y => y.UserId, (x, y) => new { X = x, UserInfo = y })
        .Select(x => new RecommendedFromFollowersDto
        {
          UserId = x.UserInfo.UserId,
          BirthDate = x.UserInfo.BirthDate,
          Login = x.UserInfo.Login,
          SexType = x.UserInfo.SexType,
          OriginalAvatarUrl = x.UserInfo.OriginalAvatarUrl,
          CommonFollowingsCount = _db.UserRelationshipEntities.Where(i => i.UserId == x.UserInfo.UserId).GroupJoin(_db.UserRelationshipEntities.Where(o => o.UserId == userId), i => i.StaredUserId, o => o.StaredUserId, (i, o) => 1).Count()

        })
        .OrderByDescending(x => x.CommonFollowingsCount).Skip(offset).Take(count).ToListAsync();
    }

    /// <summary>
    ///   возввращает список пользователей, с теми же подписками, что и у тебя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<List<RecommendedFromFollowingsDto>> GetRecommendedFromFollowings(int userId, int offset, int count)
    {
      return await _db.UserRelationshipEntities.Where(x => x.UserId == userId && x.IsFollowing)
        .Join(_db.UserRelationshipEntities.Where(x => x.StaredUserId == userId).DefaultIfEmpty(), x => x.StaredUserId,
          y => y.UserId, (x, y) => new { X = x, IsYouFollowed = y }).Where(x => !x.IsYouFollowed.IsFollowing)
        .Join(_db.UserInfoEntities, x => x.X.UserId, y => y.UserId, (x, y) => new { X = x, UserInfo = y })
        .Select(x => new RecommendedFromFollowingsDto
        {
          UserId = x.UserInfo.UserId,
          BirthDate = x.UserInfo.BirthDate,
          Login = x.UserInfo.Login,
          SexType = x.UserInfo.SexType,
          OriginalAvatarUrl = x.UserInfo.OriginalAvatarUrl,
          CommonFollowingsCount = _db.UserRelationshipEntities.Where(i => i.UserId == x.UserInfo.UserId).GroupJoin(_db.UserRelationshipEntities.Where(o => o.UserId == userId), i => i.StaredUserId, o => o.StaredUserId, (i, o) => 1).Count()

        })
        .OrderByDescending(x => x.CommonFollowingsCount).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<RecommendedFromUsersIdDto>> GetRecommendedFromUsersId(IEnumerable<int> userIds)
    {
      return await _db.UserInfoEntities.Where(x => userIds.Contains(x.UserId)).Select(x => new RecommendedFromUsersIdDto
      {
        UserId = x.UserId,
        BirthDate = x.BirthDate,
        Login = x.Login,
        OriginalAvatarUrl = x.OriginalAvatarUrl,
        SexType = x.SexType
      }).ToListAsync();

    }

    ///// <summary>
    /////   возвращает список пользователей, подписанных на тебя.
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <param name="offset"></param>
    ///// <param name="count"></param>
    ///// <returns></returns>
    //public async Task<List<RecommendedFromFollowersDb>> GetRecommendedFromFollowers(int userId, int offset, int count)
    //{
    //  return await _db.RecommendedFromFollowersDb.AsNoTracking()
    //    .FromSql($"select * from dbo.[getRecommendedFromFollowers]({userId})").OrderByDescending(x => x.CommonFollowingsCount)
    //    .Skip(offset).Take(count).ToListAsync();
    //}

    ///// <summary>
    /////   возввращает список пользователей, с теми же подписками, что и у тебя
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <param name="offset"></param>
    ///// <param name="count"></param>
    ///// <returns></returns>
    //public async Task<List<RecommendedFromFollowsDb>> GetRecommendedFromFollows(int userId, int offset, int count)
    //{
    //  return await _db.RecommendedFromFollowsDb.AsNoTracking()
    //    .FromSql($"select * from dbo.getRecommendedFromFollows({userId})").OrderByDescending(x => x.CommonFollowingsCount)
    //    .Skip(offset).Take(count).ToListAsync();
    //}

    //public async Task<List<RecommendedFromUsersIdDb>> GetRecommendedFromUsersId(IEnumerable<int> userIds)
    //{
    //  var dataTable = new DataTable();
    //  dataTable.Columns.Add("id", typeof(int));
    //  foreach (var id in userIds) dataTable.Rows.Add(id);
    //  var networkIdTable = new SqlParameter
    //  {
    //    ParameterName = "@NetworkIDTable",
    //    TypeName = "dbo.idTable",
    //    Value = dataTable
    //  };

    //  return await _db.RecommendedFromUsersIdsDb.AsNoTracking()
    //    .FromSql($"select * from dbo.getRecommendedFromUsersID(@NetworkIDTable)", networkIdTable).ToListAsync();
    //}


  }
}