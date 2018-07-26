using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators
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

    private string GetInsertionCityLine(CityQuery city)
    {
      return
        $"EXECUTE [dbo].[addCity]  N'{city.Name}'";
    }

    private string GetInsertionCitysLine(IList<CityQuery> citys)
    {
      var result = new StringBuilder();
      for (var i = 0; i < citys.Count - 1; i++)
        result.Append(GetInsertionCityLine(citys[i]) + ";" + Environment.NewLine);
      result.Append(GetInsertionCityLine(citys[citys.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<CityQuery> citys)
    {
      var result = new StringBuilder();
      var times = citys.Count < 1000 ? 1 : citys.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var citysIter = citys.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionCitysLine(citysIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}