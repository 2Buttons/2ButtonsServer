using System;
using System.Threading.Tasks;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main.Repositories
{
  public class UserInfoRepository : IDisposable
  {
    private readonly TwoButtonsContext _db;

    public UserInfoRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<UserInfoDb> FindUserInfoAsync(int userId)
    {
      return await _db.UserInfoDb.FindAsync(userId);
    }

    public void Dispose()
    {
      _db?.Dispose();
    }
  }
}