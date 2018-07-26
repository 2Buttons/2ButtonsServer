using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class QuestionFeedbackQuery
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public QuestionFeedbackType QuestionFeedbackType { get; set; }
  }
}