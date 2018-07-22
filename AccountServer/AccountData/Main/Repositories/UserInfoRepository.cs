using System.Linq;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountData.Main.Entities;
using AccountData.Main.Queries;
using CommonLibraries.Exceptions.ApiExceptions;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main.Repositories
{
  public class UserInfoRepository
  {
    private readonly TwoButtonsContext _db;

    public UserInfoRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<string> GetUserAvatar(int userId)
    {
      var user = await _db.UserInfoEntities.FirstOrDefaultAsync(x=>x.UserId == userId);
      return user?.OriginalAvatarUrl;
    }

    public async Task<UserInfoDb> FindUserInfoAsync(int userId, int userPageId)
    {
      var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
        .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found");
      if (userId != userPageId && user.IsYouFollowed) await UpdateVisitsAsync(userId, userPageId);
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      var isExist = _db.CityEntities.Any(x => x.Name == user.City);
      if (isExist) return true;
      _db.CityEntities.Add(new CityEntity {Name = user.City, Inhabitants = 1});
      await _db.SaveChangesAsync();

      var oldUserData = await _db.UserInfoDb.FirstOrDefaultAsync(x => x.UserId == user.UserId);

      oldUserData.Login = user.Login;
      oldUserData.BirthDate = user.BirthDate;
      oldUserData.SexType = user.SexType;
      oldUserData.Description = user.Description;
      oldUserData.OriginalAvatarUrl = user.OriginalAvatarUrl;
      oldUserData.City = user.City;

      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUserOriginalAvatar(int userId, string originalAvatarUrl)
    {
      var user = _db.UserInfoEntities.FirstOrDefault(x => x.UserId == userId);
      if (user == null) return false;
      user.OriginalAvatarUrl = originalAvatarUrl;
      return await _db.SaveChangesAsync() > 0;
    }


    public async Task<UserStatisticsDb> GetUserStatisticsAsync(int userId)
    {
      return await _db.UserStatisticsDb.FromSql($"select * from dbo.getUserStatistics({userId})")
               .FirstOrDefaultAsync() ?? new UserStatisticsDb();
    }

    public async Task<bool> UpdateVisitsAsync(int userId, int getUserId)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateVisits {userId}, {getUserId}") > 0;
    }
  }
}