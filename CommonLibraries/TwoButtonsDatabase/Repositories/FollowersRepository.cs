using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Followers;

namespace TwoButtonsDatabase.Repositories
{
  public class FollowersRepository
  {
    private readonly TwoButtonsContext _db;

    public FollowersRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public bool TryGetFollowers(int loggedId, int userId, int offset, int count,
      string searchedLogin,
      out IEnumerable<FollowerDb> followers)
    {
      try
      {
        followers = _db.FollowerDb
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

    public bool TryGetFollowTo(int loggedId, int userId, int offset, int count,
      string searchedLogin,
      out IEnumerable<FollowToDb> followers)
    {
      try
      {
        if (loggedId == userId)
          followers = _db.FolloToDb
            .FromSql($"select * from dbo.getFollowTo({loggedId}, {userId}, {searchedLogin})")
            .OrderBy(x => x.Visits).Skip(offset).Take(count)
            .ToList();
        else
          followers = _db.FolloToDb
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

    public bool TryGetNewFollowers(int userId,
      out IEnumerable<NewFollowersDb> newFollowers)
    {
      try
      {
        newFollowers = _db.NewFollowersDb
          .FromSql($"select * from dbo.getNewFollowers({userId})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newFollowers = new List<NewFollowersDb>();
      return false;
    }

    public bool TryAddFollow(int followerId, int followToId)
    {
      var followDate = DateTime.UtcNow;
      try
      {
        _db.Database.ExecuteSqlCommand($"addFollow {followerId}, {followToId}, {followDate}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryDeleteFollow(int followerId, int followToId)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"deleteFollow {followerId}, {followToId}");
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