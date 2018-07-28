using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    public async Task<List<RecommendedFromFollowersDb>> GetRecommendedFromFollowers(int userId, int offset, int count)
    {
      return await _db.RecommendedFromFollowersDb.AsNoTracking()
        .FromSql($"select * from dbo.[getRecommendedFromFollowers]({userId})").OrderByDescending(x => x.CommonFollowingsCount)
        .Skip(offset).Take(count).ToListAsync();
    }

    /// <summary>
    ///   возввращает список пользователей, с теми же подписками, что и у тебя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<List<RecommendedFromFollowsDb>> GetRecommendedFromFollows(int userId, int offset, int count)
    {
      return await _db.RecommendedFromFollowsDb.AsNoTracking()
        .FromSql($"select * from dbo.getRecommendedFromFollows({userId})").OrderByDescending(x => x.CommonFollowingsCount)
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<RecommendedFromUsersIdDb>> GetRecommendedFromUsersId(IEnumerable<int> userIds)
    {
      var dataTable = new DataTable();
      dataTable.Columns.Add("id", typeof(int));
      foreach (var id in userIds) dataTable.Rows.Add(id);
      var networkIdTable = new SqlParameter
      {
        ParameterName = "@NetworkIDTable",
        TypeName = "dbo.idTable",
        Value = dataTable
      };

      return await _db.RecommendedFromUsersIdsDb.AsNoTracking()
        .FromSql($"select * from dbo.getRecommendedFromUsersID(@NetworkIDTable)", networkIdTable).ToListAsync();
    }


    public async Task<List<RecommendedStrangersDb>> GetRecommendedStrangers(int userId, int offset, int count,
      string searchedLogin)
    {
      return await _db.RecommendedStrangersDb.AsNoTracking()
        .FromSql($"select * from dbo.getRecommendedStrangers({userId},   {searchedLogin})").Skip(offset).Take(count)
        .ToListAsync();
    }
  }
}