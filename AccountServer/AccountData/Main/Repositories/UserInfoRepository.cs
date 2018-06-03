using System.Linq;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountData.Main.Entities;
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

    public async Task<Info> GetUserInfoAsync(int userId, int userPageId)
    {
   
      var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
                   .FirstOrDefaultAsync() ?? new Info();

      if (userId != userPageId && user.YouFollowed) UpdateVisitsAsync(userId, userPageId);
      return user;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserTableData {user.UserId}, {user.Login}, {user.BirthDate}, {user.Sex}, {user.City},  {user.Description}, {user.LargeAvatarLink}, {user.SmallAvatarLink}") > 0;
    }

    public async Task<bool> UpdateUserLargeAvatar(int userId, string fullAvatarLink)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserFullAvatar {userId}, {fullAvatarLink}") > 0;
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