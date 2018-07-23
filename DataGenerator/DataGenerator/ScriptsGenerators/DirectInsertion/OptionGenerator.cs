using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;

namespace DataGenerator.ScriptsGenerators.DirectInsertion
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Options]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[Options] ([optionID], [questionID], [Text], [position], [AnswersCount]) VALUES" +
             Environment.NewLine;
    }

    private string GetInsertionOptionLine(OptionEntity option)
    {
      return $"({option.OptionId}, {option.QuestionId}, N'{option.Text}', {option.Position}, {option.AnswersCount})";
    }

    private string GetInsertionOptionsLine(IList<OptionEntity> options)
    {
      var result = new StringBuilder();
      for (var i = 0; i < options.Count - 1; i++)
        result.Append(GetInsertionOptionLine(options[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionOptionLine(options[options.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<OptionEntity> options)
    {
      var result = new StringBuilder();
      var times = options.Count < 1000 ? 1 : options.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var coptionsIter = options.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Options] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(true));
        result.Append(GetGo());
        result.Append(GetInsertInit());
        result.Append(GetInsertionOptionsLine(coptionsIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(false));
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Options] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}