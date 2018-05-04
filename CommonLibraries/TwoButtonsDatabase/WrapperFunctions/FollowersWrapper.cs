using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Followers;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public class FollowersWrapper
    {
      public static bool TryGetFollowers(TwoButtonsContext db, int loggedId, int userId, int page, int amount, string searchedLogin,
        out IEnumerable<FollowerDb> followers)
      {
        int fromLine = page * amount - amount + 1;
        int toLine = page * amount;
        try
        {
          followers = db.FollowerDb
            .FromSql($"select * from dbo.getFollowers({loggedId}, {userId}, {fromLine}, {toLine}, {searchedLogin})").ToList();
          return true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        followers = new List<FollowerDb>();
        return false;

      }

      public static bool TryGetFollowTo(TwoButtonsContext db, int loggedId, int userId, int page, int amount, string searchedLogin,
        out IEnumerable<FollowerDb> followers)
      {
        int fromLine = page * amount - amount + 1;
        int toLine = page * amount;

        try
        {
          followers = db.FollowerDb
            .FromSql($"select * from dbo.getFollowTo({loggedId}, {userId}, {fromLine}, {toLine}, {searchedLogin})").ToList();
          return true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        followers = new List<FollowerDb>();
        return false;

      }

      public static bool TryAddFollow(TwoButtonsContext db, int followerId, int followToId)
      {
       var followDate = DateTime.UtcNow;
        try
        {
          db.Database.ExecuteSqlCommand($"addFollow {followerId}, {followToId}, {followDate}");
          return true;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        return false;
      }

    public static bool TryDeleteFollow(TwoButtonsContext db, int followerId, int followToId)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"deleteFollow {followerId}, {followToId}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}
