using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MediaDataLayer.Repositories
{
  public class AccountRepository
  {
    private readonly TwoButtonsContext _db;

    public AccountRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> UpdateUserFullAvatar(int userId, string fullAvatarLink)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserFullAvatar {userId}, {fullAvatarLink}") > 0;
    }

    public async Task<bool> UpdateUserSmallAvatar(int userId, string smallAvatar)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateUserSmallAvatar {userId}, {smallAvatar}") > 0;
    }
  }
}