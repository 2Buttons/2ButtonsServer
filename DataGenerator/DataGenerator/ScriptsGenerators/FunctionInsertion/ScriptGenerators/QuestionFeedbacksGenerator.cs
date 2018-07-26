using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators
{
  public class QuestionFeedbackGenerator
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

    private string GetInsertionQuestionFeedbackLine(QuestionFeedbackQuery questionFeedback)
    {
      return
        $"EXECUTE [dbo].[updateQuestionFeedback] {questionFeedback.UserId}, {questionFeedback.QuestionId}, {(int)questionFeedback.QuestionFeedbackType}";
    }

    private string GetInsertionQuestionFeedbacksLine(IList<QuestionFeedbackQuery> questionFeedbacks)
    {
      var result = new StringBuilder();
      for (var i = 0; i < questionFeedbacks.Count - 1; i++)
        result.Append(GetInsertionQuestionFeedbackLine(questionFeedbacks[i]) + ";" + Environment.NewLine);
      result.Append(GetInsertionQuestionFeedbackLine(questionFeedbacks[questionFeedbacks.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<QuestionFeedbackQuery> questionFeedbacks)
    {
      var result = new StringBuilder();
      var times = questionFeedbacks.Count < 1000 ? 1 : questionFeedbacks.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var questionFeedbacksIter = questionFeedbacks.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionQuestionFeedbacksLine(questionFeedbacksIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}