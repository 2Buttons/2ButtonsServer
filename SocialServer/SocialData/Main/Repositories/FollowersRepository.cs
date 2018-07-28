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
      return await _db.FollowerDb.AsNoTracking()
        .FromSql($"select * from dbo.getFollowers({loggedId}, {userId}, {searchedLogin})").Skip(offset).Take(count)
        .ToListAsync();
    }

    public async Task<List<FollowToDb>> GetFollowTo(int userId, int userPageId, int offset, int count,
      string searchedLogin)
    {
      return await _db.FolloToDb.AsNoTracking()
        .FromSql($"select * from dbo.getFollowings({userId}, {userPageId}, {searchedLogin})")
        .OrderByDescending(x => x.VisitsCount).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<FollowToDb>> GetFollowTo(int userId, int offset, int count, string searchedLogin)
    {
      return await _db.FolloToDb.AsNoTracking()
        .FromSql($"select * from dbo.getFollowings({userId}, {userId}, {searchedLogin})").OrderBy(x => x.VisitsCount)
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<NewFollowersDb>> GetNewFollowers(int userId)
    {
      return await _db.NewFollowersDb.AsNoTracking().FromSql($"select * from dbo.getNewFollowers({userId})")
        .ToListAsync();
    }

    public async Task<bool> AddFollow(int followerId, int followToId)
    {
      var followDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync($"addFollow {followerId}, {followToId}, {followDate}") > 0;
    }

    public async Task<bool> DeleteFollow(int followerId, int followToId)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"deleteFollow {followerId}, {followToId}") > 0;
    }
  }
}