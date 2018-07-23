using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion
{
  public class QuestionGenerator
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

    private string GetInsertionQuestionLine(QuestionQuery question)
    {
      var isAnonymous = question.IsAnonymous ? 1 : 0;
      return
        $"EXECUTE [dbo].[addQuestionWithId] {question.QuestionId}, {question.UserId},  N'{question.Condition}',  N'{question.OriginalBackgroundUrl}', {isAnonymous}, {(int)question.AudienceType}, {(int)question.QuestionType}, N'{question.FirstOption}', N'{question.SecondOption}', '{question.AddedDate:u}'";
    }

    private string GetInsertionQuestionsLine(IList<QuestionQuery> questions)
    {
      var result = new StringBuilder();
      for (var i = 0; i < questions.Count - 1; i++)
        result.Append(GetInsertionQuestionLine(questions[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionQuestionLine(questions[questions.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<QuestionQuery> questions)
    {
      var result = new StringBuilder();
      var times = questions.Count < 1000 ? 1 : questions.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var questionsIter = questions.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionQuestionsLine(questionsIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}