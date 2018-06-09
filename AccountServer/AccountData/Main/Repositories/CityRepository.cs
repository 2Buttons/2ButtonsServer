using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main.Repositories
{
  public class CityRepository
  {
    private readonly TwoButtonsContext _db;

    public CityRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddCityAsync(CityEntity city)
    {
      _db.CityEntities.Add(city);
      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<CityEntity> FindCity(int cityId)
    {
      return await _db.CityEntities.FirstOrDefaultAsync(x=>x.CityId == cityId);
    }

    public async Task<List<CityEntity>> FindCityies(string pattern, int offset, int count)
    {
      return await _db.CityEntities.Where(x => x.Name.Contains(pattern)).OrderByDescending(x=>x.People).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<CityEntity>> GetPopularCity(int offset, int count)
    {
      return await _db.CityEntities.OrderByDescending(x => x.People).Skip(offset).Take(count).ToListAsync();
    }
  }
}