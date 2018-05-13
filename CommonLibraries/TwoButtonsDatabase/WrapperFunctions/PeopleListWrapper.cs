using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class PeopleListWrapper
  {
    public static bool TryGetRecommendedFromContacts(TwoButtonsContext db, int userId, string searchedLogin,
      out IEnumerable<RecommendedFromContactsDb> recommendedFromContacts)
    {
      try
      {
        recommendedFromContacts = db.RecommendedFromContactsDb
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

    public static bool TryGetRecommendedFromFollows(TwoButtonsContext db, int userId, string searchedLogin,
      out IEnumerable<RecommendedFromFollowsDb> recommendedFromFollows)
    {
      try
      {
        recommendedFromFollows = db.RecommendedFromFollowsDb
          .FromSql($"select * from dbo.getRecommendedFromFollows({userId}, {searchedLogin})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedFromFollows = new List<RecommendedFromFollowsDb>();
      return false;
    }

    public static bool TryGetRecommendedStrangers(TwoButtonsContext db, int userId, int offset, int count, string searchedLogin,
      out IEnumerable<RecommendedStrangersDb> recommendedStrangers)
    {
      

      try
      {
        recommendedStrangers = db.RecommendedStrangersDb
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

    public static bool TryGetRecommendedFromUsersId(TwoButtonsContext db, IEnumerable<int> userIds, out IEnumerable<RecommendedFromUsersIdDb> recommendedStrangers)
    {

      try
      {
        var dataTable = new DataTable();
        dataTable.Columns.Add("id", typeof(int));
        foreach (var id in userIds)
        {
          dataTable.Rows.Add(id);
        }
        var networkIdTable = new SqlParameter
        {
          ParameterName = "@NetworkIDTable",
          TypeName = "dbo.idTable",
          Value = dataTable
        };

        recommendedStrangers = db.RecommendedFromUsersIdsDb
          .FromSql($"select * from dbo.[getRecommendedFromUsersID](@NetworkIDTable)", networkIdTable)
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
  }
}