using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using SocialData.Main.Entities;
using SocialData.Main.Entities.Followers;


namespace SocialData.Main.Repositories
{
  public class FollowersRepository
  {
    private readonly TwoButtonsContext _db;

    public FollowersRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<List<FollowerDb>> GetFollowers(int loggedId, int userId, int offset, int count,
      string searchedLogin)
    {
      try
      {
        return await _db.FollowerDb.AsNoTracking()
          .FromSql($"select * from dbo.getFollowers({loggedId}, {userId}, {searchedLogin})")
          .Skip(offset).Take(count)
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<FollowerDb>();
    }

    public async Task<List<FollowToDb>> GetFollowTo(int userId, int userPageId, int offset, int count,
      string searchedLogin)
    {
      try
      {
        return await _db.FolloToDb.AsNoTracking()
           .FromSql($"select * from dbo.getFollowTo({userId}, {userPageId}, {searchedLogin})")
           .Skip(offset).Take(count)
           .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<FollowToDb>();
    }

    public async Task<List<FollowToDb>> GetFollowTo(int userId, int offset, int count,
      string searchedLogin)
    {
      try
      {

        return await _db.FolloToDb.AsNoTracking()
          .FromSql($"select * from dbo.getFollowTo({userId}, {userId}, {searchedLogin})")
          .OrderBy(x => x.Visits).Skip(offset).Take(count)
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<FollowToDb>();
    }

    public async Task<List<NewFollowersDb>> GetNewFollowers(int userId)
    {
      try
      {
        return await _db.NewFollowersDb.AsNoTracking()
          .FromSql($"select * from dbo.getNewFollowers({userId})").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<NewFollowersDb>();
    }

    public async Task<bool> AddFollow(int followerId, int followToId)
    {
      var followDate = DateTime.UtcNow;
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"addFollow {followerId}, {followToId}, {followDate}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<bool> DeleteFollow(int followerId, int followToId)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"deleteFollow {followerId}, {followToId}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}