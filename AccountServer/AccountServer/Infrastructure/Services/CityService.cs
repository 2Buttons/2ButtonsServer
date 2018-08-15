using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Main.Entities;
using CommonLibraries.Entities.Main;
using Microsoft.Extensions.Logging;

namespace AccountServer.Infrastructure.Services
{
  public class CityService : ICityService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly  ILogger<AccountService> _logger;

    public CityService(AccountDataUnitOfWork accountDb, ILogger<AccountService> logger)
    {
      _db = accountDb;
      _logger = logger;
    }

    public async Task<List<CityEntity>> GetPopularCities(int offset, int count)
    {
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetPopularCities)}.Start");
      var result =   await _db.Cities.GetPopularCity(offset, count);
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetPopularCities)}.End");
      return result;
    }

    public async Task<List<CityEntity>> FindCities(string pattern, int offset, int count)
    {
      _logger.LogInformation($"{nameof(CityService)}.{nameof(FindCities)}.Start");
      var result =  await _db.Cities.FindCityies(pattern, offset, count);
      _logger.LogInformation($"{nameof(CityService)}.{nameof(FindCities)}.End");
      return result;
    }
  }
}