using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialData.Main;
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
      try
      {
        return await _db.RecommendedFromFollowersDb.AsNoTracking()
          .FromSql($"select * from dbo.[getRecommendedFromFollowers]({userId})")
          .OrderByDescending(x => x.CommonFollowsTo).Skip(offset).Take(count).ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<RecommendedFromFollowersDb>();
    }

    /// <summary>
    ///   возввращает список пользователей, с теми же подписками, что и у тебя
    /// </summary>
    /// <param name="db"></param>
    /// <param name="userId"></param>
    /// <param name="recommendedFromFollows"></param>
    /// <returns></returns>
    public async Task<List<RecommendedFromFollowsDb>> GetRecommendedFromFollows(int userId, int offset, int count)
    {
      try
      {
        return await _db.RecommendedFromFollowsDb.AsNoTracking()
          .FromSql($"select * from dbo.getRecommendedFromFollows({userId})").OrderByDescending(x => x.CommonFollowsTo)
          .Skip(offset).Take(count).ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<RecommendedFromFollowsDb>();
    }

    public async Task<List<RecommendedFromUsersIdDb>> GetRecommendedFromUsersId(IEnumerable<int> userIds)
    {
      try
      {
        var dataTable = new DataTable();
        dataTable.Columns.Add("id", typeof(int));
        foreach (var id in userIds)
          dataTable.Rows.Add(id);
        var networkIdTable = new SqlParameter
        {
          ParameterName = "@NetworkIDTable",
          TypeName = "dbo.idTable",
          Value = dataTable
        };

        return await _db.RecommendedFromUsersIdsDb.AsNoTracking()
          .FromSql($"select * from dbo.getRecommendedFromUsersID(@NetworkIDTable)", networkIdTable)
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
     return new List<RecommendedFromUsersIdDb>();
    }

    public async Task<List<RecommendedFromContactsDb>> GetRecommendedFromContacts(int userId, string searchedLogin)
    {
      try
      {
        return await _db.RecommendedFromContactsDb.AsNoTracking()
          .FromSql($"select * from dbo.getRecommendedFromContacts({userId}, {searchedLogin})").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<RecommendedFromContactsDb>();
    }

    public async Task<List<RecommendedStrangersDb>> GetRecommendedStrangers(int userId, int offset, int count, string searchedLogin)
    {
      try
      {
        return await _db.RecommendedStrangersDb.AsNoTracking()
          .FromSql($"select * from dbo.getRecommendedStrangers({userId},   {searchedLogin})")
          .Skip(offset).Take(count)
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return  new List<RecommendedStrangersDb>();
    }
  }
}