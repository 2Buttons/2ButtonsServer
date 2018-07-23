using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class QuestionQuery
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Condition { get; set; }
    public string FirstOption { get; set; }
    public string SecondOption { get; set; }
    public bool IsAnonymous { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    public string OriginalBackgroundUrl { get; set; }
    public DateTime AddedDate { get; set; }
  }
}