using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.Recommended;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public class RecommendedSubscribersWrapper
    {
    /// <summary>
    /// возвращает список пользователей, подписанных на тебя.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="userId"></param>
    /// <param name="recommendedFromFollows"></param>
    /// <returns></returns>
    public static bool TryGetRecommendedFromFollowers(TwoButtonsContext db, int userId,
        out IEnumerable<RecommendedFromFollowersDb> recommendedFromFollows)
      {
        try
        {
          recommendedFromFollows = db.RecommendedFromFollowersDb
            .FromSql($"select * from dbo.[getRecommendedFromFollowers]({userId})").OrderByDescending(x=>x.CommonFollowsTo).ToList();
          return true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        recommendedFromFollows = new List<RecommendedFromFollowersDb>();
        return false;
      }

    /// <summary>
    /// возввращает список пользователей, с теми же подписками, что и у тебя
    /// </summary>
    /// <param name="db"></param>
    /// <param name="userId"></param>
    /// <param name="recommendedFromFollows"></param>
    /// <returns></returns>
    public static bool TryGetRecommendedFromFollows(TwoButtonsContext db, int userId,
        out IEnumerable<RecommendedFromFollowsDb> recommendedFromFollows)
      {
        try
        {
          recommendedFromFollows = db.RecommendedFromFollowsDb
            .FromSql($"select * from dbo.getRecommendedFromFollows({userId})").OrderByDescending(x => x.CommonFollowsTo).ToList();
          return true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        recommendedFromFollows = new List<RecommendedFromFollowsDb>();
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
    }
}
