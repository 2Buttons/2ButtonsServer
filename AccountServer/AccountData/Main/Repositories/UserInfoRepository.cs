using System.Linq;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountData.Main.Entities;
using AccountData.Main.Queries;
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

    public async Task<UserInfoDb> GetUserInfoAsync(int userId, int userPageId)
    {
      var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
        .FirstOrDefaultAsync();
      if (user == null) return null;
      if (userId != userPageId && user.YouFollowed) await UpdateVisitsAsync(userId, userPageId);
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      var isExist = _db.CityEntities.Any(x => x.Name == user.City);
      if (isExist) return true;
      _db.CityEntities.Add(new CityEntity {Name = user.City, People = 1});
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

    public async Task<bool> UpdateUserLargeAvatar(int userId, string fullAvatarUrl)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserFullAvatar {userId}, {fullAvatarUrl}") > 0;
    }

    public async Task<bool> UpdateUserSmallAvatar(int userId, string smallAvatar)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserSmallAvatar {userId}, {smallAvatar}") > 0;
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