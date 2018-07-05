﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class AnswerGenerator
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Answer]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO[dbo].[Anwser] ([userID], [questionID], [anwser], [liked], [anwserDate], [deleted]) VALUES" +
             Environment.NewLine;
    }

    private string GetInsertionAnswerLine(AnswerEntity answer)
    {
      var isLiked = answer.IsLiked ? 1 : 0;
      return $"({answer.UserId}, {answer.QuestionId}, {(int)answer.AnswerType}, {isLiked}, '{answer.AnswerDate}', 0)";
    }

    private string GetInsertionAnswersLine(IList<AnswerEntity> answers)
    {
      var result = new StringBuilder();
      for (var i = 0; i < answers.Count - 1; i++)
        result.Append(GetInsertionAnswerLine(answers[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionAnswerLine(answers[answers.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<AnswerEntity> answers)
    {
      var result = new StringBuilder();
      var times = answers.Count < 1000 ? 1 : answers.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var canswersIter = answers.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        //result.Append("ALTER TABLE [dbo].[Answer] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        //result.Append(GetGo());
        result.Append(SwitchIdentityInsert(true));
        result.Append(GetGo());
        result.Append(GetInsertInit());
        result.Append(GetInsertionAnswersLine(canswersIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(false));
        result.Append(GetGo());
        //result.Append("ALTER TABLE [dbo].[Answer] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        //result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}