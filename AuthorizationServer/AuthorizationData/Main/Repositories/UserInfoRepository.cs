using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AuthorizationData.Main.Entities;
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

    public async Task<bool> AddUserInfoAsync(UserInfoDb user)
    {
      var returnsCode = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};
      return await _contextMain.Database.ExecuteSqlCommandAsync( $"addUser {user.UserId}, {user.Login}, {user.BirthDate}, {user.SexType}, {user.City},  {user.Description}, {user.LargeAvatarLink}, {user.SmallAvatarLink}, {returnsCode} OUT") > 0;
    }

    public async Task<bool> UpdateUserInfoAsync(UserInfoDb user)
    {
      return await _contextMain.Database.ExecuteSqlCommandAsync( $"updateUserTableData {user.UserId}, {user.Login}, {user.BirthDate}, {user.SexType}, {user.City},  {user.Description}, {user.LargeAvatarLink}, {user.SmallAvatarLink}") > 0;
    }

    public async Task<UserInfoDb> GetUserInfoAsync(int userId)
    {
     return await _contextMain.UsersInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserTableData({userId})").FirstOrDefaultAsync() ?? new UserInfoDb();
    }
  }
}