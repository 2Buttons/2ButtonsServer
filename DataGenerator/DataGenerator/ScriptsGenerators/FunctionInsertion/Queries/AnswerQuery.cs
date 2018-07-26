using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class AnswerQuery
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public AnswerType AnswerType { get; set; }
    public DateTime AnsweredDate { get; set; }
  }
}

