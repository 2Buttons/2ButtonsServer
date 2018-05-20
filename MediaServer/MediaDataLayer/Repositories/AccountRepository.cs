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
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateUserFullAvatar {userId}, {fullAvatarLink}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<bool> UpdateUserSmallAvatar(int userId, string smallAvatar)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateUserSmallAvatar {userId}, {smallAvatar}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}