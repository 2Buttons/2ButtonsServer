using System.Collections.Generic;

namespace DataGenerator.Data.Reader.Objects
{
  public class QuestionDescriptionReader
  {
    public string BackgroundUrl { get; set; }
    public List<string> Tags { get; set; }
    public List<int> QuestionIds { get; set; }
  }
}