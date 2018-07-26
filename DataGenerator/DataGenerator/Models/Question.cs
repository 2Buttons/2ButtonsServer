using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;

namespace DataGenerator.Models
{
  public class Question
  {
    public int QuestionId { get; set; }
    public int AuthorId { get; set; }
    public string Condition { get; set; }
    public bool IsAnonymous { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    public string FirstOption { get; set; }
    public int FirstOptionPercentCount { get; set; }
    public string SecondOption { get; set; }
    public int SecondOptionPercentCount { get; set; }
    public string BackgroundUrl { get; set; }
    public DateTime AddedDate { get; set; }
    public int AnswersCount { get; set; }
    public List<string> Tags { get; set; } = new List<string>();

  }
}
