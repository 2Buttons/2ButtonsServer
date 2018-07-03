namespace DataGenerator.ScriptsGenerators.Entities
{
  public class OptionEntity
  {
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string OptionText { get; set; }
    public int Position { get; set; }
    public int Answers { get; set; }
  }
}