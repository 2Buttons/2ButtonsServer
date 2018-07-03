using System;
using System.Collections.Generic;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class CityGenerator
  {
    private string _db = "TwoButtons";

    public CityGenerator()
    {

    }

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine+"GO" + Environment.NewLine;
    }

    private string SwitchIdentityInsert(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[City]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[City] ([cityID], [name], [people]) VALUES"+ Environment.NewLine;
    }

    private string GetUpdatingInit()
    {
      return "UPDATE [dbo].[City] SET"+Environment.NewLine;
    }

    private string GetUpdatingSetLine(CityEntity city)
    {
      return $"[name] = '{city.Name}',[people] {city.People}"+ Environment.NewLine;
    }

    private string GetUpdatingWhere(int cityId)
    {
      return $"WHERE [dbo].[City].[CityId] = {cityId}+Environment.NewLine";
    }

    private string GetUpdatingCityLine(CityEntity city)
    {
      return GetUpdatingInit() + GetUpdatingSetLine(city) + GetUpdatingWhere(city.CityId)+ Environment.NewLine;
    }

    private string GetInsertionCityLine(CityEntity city)
    {
      return $"({city.CityId}, '{city.Name}', {city.People})";
    }

    private string GetInsertionCitiesLine(IList<CityEntity> cities)
    {
      StringBuilder result = new StringBuilder();
      for (int i = 0; i < cities.Count-1; i++)
      {
        result.Append(GetInsertionCityLine(cities[i])+","+ Environment.NewLine);
      }
      result.Append(GetInsertionCityLine(cities[cities.Count-1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<CityEntity> cities)
    {
      StringBuilder result = new StringBuilder();
      result.Append(GetUsingDb());
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(true));
      result.Append(GetGo());
      result.Append(GetInsertInit());
      result.Append(GetInsertionCitiesLine(cities));
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(false));
      result.Append(GetGo());
      return result.ToString();
    }

    public string GetUpdatingLine(IList<CityEntity> cities)
    {
      StringBuilder result = new StringBuilder();
      result.Append(GetUsingDb());
      result.Append(GetGo());
      foreach (var city in cities)
      {
        result.Append(GetUpdatingCityLine(city));
      }
      result.Append(GetGo());
      return result.ToString();
    }

  }
}
