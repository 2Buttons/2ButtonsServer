using System.Collections.Generic;

namespace DataGenerator.ReaderObjects
{
  public class City : IEqualityComparer<City>
  {
    public int CityId { get; set; }
    public string Title { get; set; }

    public bool Equals(City x, City y)
    {
      return x.CityId == y.CityId && x.Title == y.Title;
    }

    public int GetHashCode(City obj)
    {
      return obj.CityId ^ obj.Title.GetHashCode() ^ 17;
    }
  }
}