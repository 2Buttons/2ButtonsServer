using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Main.Entities;

namespace AccountServer.Infrastructure.Services
{
  public class CityService : ICityService
  {
    private readonly AccountDataUnitOfWork _db;

    public CityService(AccountDataUnitOfWork accountDb)
    {
      _db = accountDb;
    }

    public async Task<List<CityEntity>> GetPopularCities(int offset, int count)
    {
      return await _db.Cities.GetPopularCity(offset, count);
    }

    public async Task<List<CityEntity>> FindCities(string pattern, int offset, int count)
    {
      return await _db.Cities.FindCityies(pattern, offset, count);
    }
  }
}