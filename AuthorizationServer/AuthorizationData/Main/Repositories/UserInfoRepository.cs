using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AuthorizationData.Main.Dto;
using AuthorizationData.Main.Entities;
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Main.Repositories
{
  public class UserInfoRepository : IDisposable
  {
    private readonly TwoButtonsContext _contextMain;

    public UserInfoRepository(TwoButtonsContext contextMain)
    {
      _contextMain = contextMain;
    }

    public void Dispose()
    {
      _contextMain.Dispose();
    }

    public async Task<bool> AddUserInfoAsync(UserInfoQuery user)
    {
      var returnsCode = new SqlParameter { SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
      return await _contextMain.Database.ExecuteSqlCommandAsync($"addUser {user.UserId}, {user.FirstName}, {user.LastName}, {user.BirthDate}, {user.SexType}, {user.City},  {user.Description}, {user.OriginalAvatarUrl}, {returnsCode} OUT") > 0;
    }

    public async Task<bool> UpdateUserInfoAsync(UserInfoEntity user)
    {
      var userChanged = await _contextMain.UserInfoEntities.FirstOrDefaultAsync(x => x.UserId == user.UserId);
      if (userChanged == null) return false;

      userChanged.FirstName = user.FirstName;
      userChanged.LastName = user.LastName;
      userChanged.BirthDate = user.BirthDate;
      userChanged.SexType = user.SexType;
      userChanged.Description = user.Description;
      userChanged.OriginalAvatarUrl = user.OriginalAvatarUrl;

      return
        await _contextMain.SaveChangesAsync() >
        0; //_contextMain.Database.ExecuteSqlCommandAsync( $"updateUserTableData {user.UserId}, {user.Login}, {user.BirthDate}, {user.SexType}, {user.City},  {user.Description}, {user.OriginalAvatarUrl}") > 0;
    }

    public async Task<UserInfoEntity> GetUserInfoAsync(int userId)
    {
     return await _contextMain.UserInfoEntities.FirstOrDefaultAsync(x=>x.UserId == userId);
    }

    public async Task<UserInfoDto> GetUserInfoWithCityAsync(int userId)
    {
      var user =  await _contextMain.UserInfoEntities.FirstOrDefaultAsync(x => x.UserId == userId);
      var city = await _contextMain.CityEntities.FirstOrDefaultAsync(x => x.CityId == user.CityId);
      var result = new UserInfoDto
      {
        UserId = user.UserId,
        FirstName = user.FirstName,
        LastName = user.LastName,
        BirthDate = user.BirthDate,
        City = city.Name,
        Description = user.Description,
        OriginalAvatarUrl = user.OriginalAvatarUrl,
        SexType = user.SexType
      };
      return result;
    }
  }
}