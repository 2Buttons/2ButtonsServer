using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData.Main.Entities;

namespace AccountServer.Infrastructure.Services
{
  public interface ICityService
  {
    Task<List<CityEntity>> FindCities(string pattern, int offset, int count);
    Task<List<CityEntity>> GetPopularCities(int offset, int count);
  }
}