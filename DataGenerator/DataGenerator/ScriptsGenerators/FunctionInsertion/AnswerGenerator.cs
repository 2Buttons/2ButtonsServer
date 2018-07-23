using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion
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

    private string GetInsertionAnswerLine(AnswerQuery answer)
    {
      return
        $"EXECUTE [dbo].[updateAnswer] {answer.UserId}, {answer.QuestionId}, {(int) answer.AnswerType}, '{answer.AnsweredDate:u}'";
    }

    private string GetInsertionAnswersLine(IList<AnswerQuery> answers)
    {
      var result = new StringBuilder();
      for (var i = 0; i < answers.Count - 1; i++)
        result.Append(GetInsertionAnswerLine(answers[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionAnswerLine(answers[answers.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<AnswerQuery> answers)
    {
      var result = new StringBuilder();
      var times = answers.Count < 1000 ? 1 : answers.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var answersIter = answers.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionAnswersLine(answersIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}