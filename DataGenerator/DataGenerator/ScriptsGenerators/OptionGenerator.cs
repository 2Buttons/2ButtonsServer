using System;
using System.Collections.Generic;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class OptionGenerator
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Option]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[Option] ([optionID], [questionID], [optionText], [position], [answers]) VALUES" +
             Environment.NewLine;
    }

    private string GetInsertionOptionLine(OptionEntity option)
    {
      return $"({option.OptionId}, {option.QuestionId}, '{option.OptionText}', {option.Position}, {option.Answers})";
    }

    private string GetInsertionOptionsLine(IList<OptionEntity> options)
    {
      var result = new StringBuilder();
      for (var i = 0; i < options.Count - 1; i++)
        result.Append(GetInsertionOptionLine(options[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionOptionLine(options[options.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<OptionEntity> cities)
    {
      var result = new StringBuilder();
      result.Append(GetUsingDb());
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(true));
      result.Append(GetGo());
      result.Append(GetInsertInit());
      result.Append(GetInsertionOptionsLine(cities));
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(false));
      result.Append(GetGo());
      return result.ToString();
    }
  }
}