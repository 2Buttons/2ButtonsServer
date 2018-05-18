using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.Recommended;

namespace TwoButtonsDatabase.Repositories
{
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
    /// <param name="db"></param>
    /// <param name="userId"></param>
    /// <param name="recommendedFromFollows"></param>
    /// <returns></returns>
    public bool TryGetRecommendedFromFollowers(int userId, int offset, int count,
      out IEnumerable<RecommendedFromFollowersDb> recommendedFromFollowers)
    {
      try
      {
        recommendedFromFollowers = _db.RecommendedFromFollowersDb
          .FromSql($"select * from dbo.[getRecommendedFromFollowers]({userId})")
          .OrderByDescending(x => x.CommonFollowsTo).Skip(offset).Take(count).ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedFromFollowers = new List<RecommendedFromFollowersDb>();
      return false;
    }

    /// <summary>
    ///   возввращает список пользователей, с теми же подписками, что и у тебя
    /// </summary>
    /// <param name="db"></param>
    /// <param name="userId"></param>
    /// <param name="recommendedFromFollows"></param>
    /// <returns></returns>
    public bool TryGetRecommendedFromFollows(int userId, int offset, int count,
      out IEnumerable<RecommendedFromFollowsDb> recommendedFromFollows)
    {
      try
      {
        recommendedFromFollows = _db.RecommendedFromFollowsDb
          .FromSql($"select * from dbo.getRecommendedFromFollows({userId})").OrderByDescending(x => x.CommonFollowsTo)
          .Skip(offset).Take(count).ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedFromFollows = new List<RecommendedFromFollowsDb>();
      return false;
    }

    public bool TryGetRecommendedFromUsersId(IEnumerable<int> userIds,
      out IEnumerable<RecommendedFromUsersIdDb> recommendedStrangers)
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

        recommendedStrangers = _db.RecommendedFromUsersIdsDb
          .FromSql($"select * from dbo.getRecommendedFromUsersID(@NetworkIDTable)", networkIdTable)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedStrangers = new List<RecommendedFromUsersIdDb>();
      return false;
    }

    public bool TryGetRecommendedFromContacts(int userId, string searchedLogin,
      out IEnumerable<RecommendedFromContactsDb> recommendedFromContacts)
    {
      try
      {
        recommendedFromContacts = _db.RecommendedFromContactsDb
          .FromSql($"select * from dbo.getRecommendedFromContacts({userId}, {searchedLogin})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedFromContacts = new List<RecommendedFromContactsDb>();
      return false;
    }

    public bool TryGetRecommendedStrangers(int userId, int offset, int count, string searchedLogin,
      out IEnumerable<RecommendedStrangersDb> recommendedStrangers)
    {
      try
      {
        recommendedStrangers = _db.RecommendedStrangersDb
          .FromSql($"select * from dbo.getRecommendedStrangers({userId},   {searchedLogin})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedStrangers = new List<RecommendedStrangersDb>();
      return false;
    }
  }
}