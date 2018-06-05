using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationsData.Main.Entities;

namespace NotificationsData.Main.Repositories
{
  public class UserInfoRepository 
  {
    private readonly TwoButtonsContext _db;

    public UserInfoRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<UserInfoDb> FindUserInfoAsync(int userId)
    {
      return (await _db.UserInfoDb.FirstOrDefaultAsync(x=>x.UserId==userId)) ?? new UserInfoDb();
    }

   
  }
}