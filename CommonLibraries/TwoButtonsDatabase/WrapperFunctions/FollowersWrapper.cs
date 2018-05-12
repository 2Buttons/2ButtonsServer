using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.Followers;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public class FollowersWrapper
  {
    public static bool TryGetFollowers(TwoButtonsContext db, int loggedId, int userId, int offset, int count,
      string searchedLogin,
      out IEnumerable<FollowerDb> followers)
    {
      
      try
      {
        followers = db.FollowerDb
          .FromSql($"select * from dbo.getFollowers({loggedId}, {userId}, {searchedLogin})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      followers = new List<FollowerDb>();
      return false;
    }

    public static bool TryGetFollowTo(TwoButtonsContext db, int loggedId, int userId, int offset, int count,
      string searchedLogin,
      out IEnumerable<FollowToDb> followers)
    {
      

      try
      {
        if(loggedId == userId)
        followers = db.FolloToDb
          .FromSql($"select * from dbo.getFollowTo({loggedId}, {userId}, {searchedLogin})")
          .OrderBy(x=>x.Visits).Skip(offset).Take(count)
          .ToList();
        else
          followers = db.FolloToDb
            .FromSql($"select * from dbo.getFollowTo({loggedId}, {userId}, {searchedLogin})")
            .Skip(offset).Take(count)
            .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      followers = new List<FollowToDb>();
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