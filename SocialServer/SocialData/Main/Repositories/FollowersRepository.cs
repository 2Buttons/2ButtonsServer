using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;
using SocialData.Main.DTO;
using SocialData.Main.Queries;

namespace SocialData.Main.Repositories
{
  public class FollowersRepository
  {
    private readonly TwoButtonsContext _db;

    public FollowersRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> Follow(int userId, int followingUserId)
    {
      var relation =
        await _db.UserRelationshipEntities.FirstOrDefaultAsync(
          x => x.UserId == userId && x.FollowingId == followingUserId);
      if (relation == null)
      {
        relation = new FollowingEntity
        {
          UserId = userId,
          FollowingId = followingUserId,
          VisitsCount = 0,
          IsFollowing = true,
          FollowingDate = DateTime.UtcNow,
          LastVisitDate = DateTime.UtcNow
        };
        _db.UserRelationshipEntities.Add(relation);
      }
      relation.IsFollowing = true;
      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> Unfollow(int userId, int followingUserId)
    {
      var relation =
        await _db.UserRelationshipEntities.FirstOrDefaultAsync(
          x => x.UserId == userId && x.FollowingId == followingUserId);
      if (relation == null || !relation.IsFollowing) return true;
      relation.IsFollowing = false;
      return await _db.SaveChangesAsync() > 0;
    }


    public async Task<List<FollowingQuery>> GetFollowers(int userId, int userPageId, int offset, int count, string searchedLogin)
    {
      if (!string.IsNullOrEmpty(searchedLogin))
        return await _db.Followings.AsNoTracking()
          .FromSql($"select * from dbo.getFollowings({userId}, {userPageId})").Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(searchedLogin.ToLower())).OrderByDescending(x => x.VisitsCount).Skip(offset).Take(count)
          .ToListAsync();
      return await _db.Followings.AsNoTracking()
        .FromSql($"select * from dbo.getFollowers({userId}, {userPageId})").OrderByDescending(x=>x.VisitsCount).Skip(offset).Take(count)
        .ToListAsync();
    }

    public async Task<List<FollowingQuery>> GetFollowings(int userId, int userPageId, int offset, int count, string searchedLogin)
    {
      if(!string.IsNullOrEmpty(searchedLogin))
        return await _db.Followings.AsNoTracking()
          .FromSql($"select * from dbo.getFollowings({userId}, {userPageId})").Where(x=> (x.FirstName + " " + x.LastName).ToLower().Contains(searchedLogin.ToLower())).OrderByDescending(x => x.VisitsCount).Skip(offset).Take(count)
          .ToListAsync();
      return await _db.Followings.AsNoTracking()
        .FromSql($"select * from dbo.getFollowings({userId}, {userPageId})").OrderByDescending(x => x.VisitsCount).Skip(offset).Take(count)
        .ToListAsync();
    }

    //public async Task<List<FollowerDto>> GetFollowers(int loggedUserId, int userPageId, int offset, int count)
    //{
    //  return await _db.UserRelationshipEntities.Where(x => x.FollowingId == userPageId && x.IsFollowing)
    //    .Join(_db.UserRelationshipEntities.Where(x => x.UserId == loggedUserId).DefaultIfEmpty(), x => x.UserId,
    //      y => y.FollowingId, (x, y) => new {X = x, IsHeFollowed = y})
    //    .OrderByDescending(x => x.IsHeFollowed.VisitsCount).Skip(offset).Take(count)
    //    .Join(_db.UserRelationshipEntities.Where(x => x.FollowingId == loggedUserId).DefaultIfEmpty(), x => x.X.UserId,
    //      y => y.UserId, (x, y) => new {X = x, IsYouFollowed = y})
    //    .Join(_db.UserInfoEntities, x => x.X.X.UserId, y => y.UserId, (x, y) => new {X = x, UserInfo = y}).Select(
    //      x => new FollowerDto
    //      {
    //        UserId = x.X.X.X.UserId,
    //        Login = x.UserInfo.Login,
    //        SexType = x.UserInfo.SexType,
    //        OriginalAvatarUrl = x.UserInfo.OriginalAvatarUrl,
    //        BirthDate = x.UserInfo.BirthDate,
    //        IsHeFollowed = x.X.X.IsHeFollowed.IsFollowing,
    //        IsYouFollowed = x.X.IsYouFollowed.IsFollowing
    //      }).ToListAsync();
    //}

    //public async Task<List<FollowingDto>> GetFollowings(int loggedUserId, int userPageId, int offset, int count)
    //{
    //  return await _db.UserRelationshipEntities.Where(x => x.UserId == userPageId && x.IsFollowing)
    //    .Join(_db.UserRelationshipEntities.Where(x => x.UserId == loggedUserId).DefaultIfEmpty(), x => x.UserId,
    //      y => y.FollowingId, (x, y) => new {X = x, IsHeFollowed = y})
    //    .OrderByDescending(x => x.IsHeFollowed.VisitsCount).Skip(offset).Take(count)
    //    .Join(_db.UserRelationshipEntities.Where(x => x.FollowingId == loggedUserId).DefaultIfEmpty(), x => x.X.UserId,
    //      y => y.UserId, (x, y) => new {X = x, IsYouFollowed = y})
    //    .Join(_db.UserInfoEntities, x => x.X.X.UserId, y => y.UserId, (x, y) => new {X = x, UserInfo = y}).Select(
    //      x => new FollowingDto
    //      {
    //        UserId = x.X.X.X.UserId,
    //        Login = x.UserInfo.Login,
    //        SexType = x.UserInfo.SexType,
    //        OriginalAvatarUrl = x.UserInfo.OriginalAvatarUrl,
    //        BirthDate = x.UserInfo.BirthDate,
    //        IsHeFollowed = x.X.X.IsHeFollowed.IsFollowing,
    //        IsYouFollowed = x.X.IsYouFollowed.IsFollowing
    //      }).ToListAsync();
    //}

    //{
    //  return await _db.FollowerDb.AsNoTracking()
    //    .FromSql($"select * from dbo.getFollowers({loggedId}, {userId}, {searchedLogin})").Skip(offset).Take(count)
    //    .ToListAsync();
    //}

    //public async Task<List<FollowerDb>> GetFollowers(int loggedId, int userId, int offset, int count,


    //public async Task<List<FollowToDb>> GetFollowTo(int userId, int offset, int count, string searchedLogin)
    //{
    //  return await _db.FolloToDb.AsNoTracking()
    //    .FromSql($"select * from dbo.getFollowings({userId}, {userId}, {searchedLogin})").OrderBy(x => x.VisitsCount)
    //    .Skip(offset).Take(count).ToListAsync();
    //}

    //public async Task<List<NewFollowersDb>> GetNewFollowers(int userId)
    //{
    //  return await _db.NewFollowersDb.AsNoTracking().FromSql($"select * from dbo.getNewFollowers({userId})")
    //    .ToListAsync();
    //}

    //public async Task<bool> AddFollow(int followerId, int followToId)
    //{
    //  var followDate = DateTime.UtcNow;

    //  return await _db.Database.ExecuteSqlCommandAsync($"addFollow {followerId}, {followToId}, {followDate}") > 0;
    //}

    //public async Task<bool> DeleteFollow(int followerId, int followToId)
    //{
    //  return await _db.Database.ExecuteSqlCommandAsync($"deleteFollow {followerId}, {followToId}") > 0;
    //}
  }
}