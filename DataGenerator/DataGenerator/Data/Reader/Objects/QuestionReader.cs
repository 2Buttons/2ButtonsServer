using System.Collections.Generic;

namespace DataGenerator.Data.Reader.Objects
{
  public class QuestionReader
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public string FirstOption { get; set; }
    public string SecondOption { get; set; }
    public int FirstAnswersPercent { get; set; }
    public int SecondAnswersPercent { get; set; }
    public List<string> Tags { get; set; }
  }
}