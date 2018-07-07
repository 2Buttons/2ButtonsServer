using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsData.Contexts;
using BotsData.Dto;
using BotsData.Entities;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;

namespace BotsData.Repositories
{
  public class BotsRepository
  {
    private readonly TwoButtonsContext _context;
    private readonly TwoButtonsAccountContext _contextAccount;

    public BotsRepository(TwoButtonsContext context, TwoButtonsAccountContext contextAccount)
    {
      _context = context;
      _contextAccount = contextAccount;
    }

    
    public async Task<List<UserEntity>> GetBots(int offset, int count)
    {
      var result =  await _contextAccount.UserEntities.Where(x => x.IsBot).Skip(offset).Take(count).ToListAsync();
      return result;
    }

    public async Task<UserEntity> GetBotsById(int botId)
    {
      return await _contextAccount.UserEntities.FirstOrDefaultAsync(x => x.UserId == botId);
    }

    public async Task<List<UserEntity>> GetAllBots()
    {
      return await _contextAccount.UserEntities.Where(x => x.IsBot).ToListAsync();
    }

    public  int GetBotsCount(int botId)
    {
      return  _contextAccount.UserEntities.Count(x => x.UserId == botId);
    }
  }
}