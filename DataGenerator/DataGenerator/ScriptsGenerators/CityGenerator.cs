﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class CityGenerator
  {
    private readonly string _db = "TwoButtons";

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string SwitchIdentityInsert(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[City]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[City] ([cityID], [name], [people]) VALUES" + Environment.NewLine;
    }

    private string GetUpdatingInit()
    {
      return "UPDATE [dbo].[City] SET" + Environment.NewLine;
    }

    private string GetUpdatingSetLine(CityEntity city)
    {
      return $"[name] = '{city.Title}',[people] {city.People}" + Environment.NewLine;
    }

    private string GetUpdatingWhere(int cityId)
    {
      return $"WHERE [dbo].[City].[CityId] = {cityId}" + Environment.NewLine;
    }

    private string GetUpdatingCityLine(CityEntity city)
    {
      return GetUpdatingInit() + GetUpdatingSetLine(city) + GetUpdatingWhere(city.CityId) + Environment.NewLine;
    }

    private string GetInsertionCityLine(CityEntity city)
    {
      return $"({city.CityId}, N'{city.Title}', {city.People})";
    }

    private string GetInsertionCitiesLine(IList<CityEntity> cities)
    {
      var result = new StringBuilder();
      for (var i = 0; i < cities.Count - 1; i++)
        result.Append(GetInsertionCityLine(cities[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionCityLine(cities[cities.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<CityEntity> cities)
    {
      var result = new StringBuilder();
      var times = cities.Count < 1000 ? 1 : cities.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var citiesIter = cities.Skip(i * 1000).Take(1000).ToList();

        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(true));
        result.Append(GetGo());
        result.Append(GetInsertInit());
        result.Append(GetInsertionCitiesLine(citiesIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(false));
        result.Append(GetGo());
      }

      return result.ToString();
    }

    public string GetUpdatingLine(IList<CityEntity> cities)
    {
      var result = new StringBuilder();
      result.Append(GetUsingDb());
      result.Append(GetGo());
      foreach (var city in cities) result.Append(GetUpdatingCityLine(city));
      result.Append(GetGo());
      return result.ToString();
    }
  }
}