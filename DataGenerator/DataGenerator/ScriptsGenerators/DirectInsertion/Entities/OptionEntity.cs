namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{
  public class OptionEntity
  {
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int Position { get; set; }
    public int AnswersCount { get; set; }
  }
}