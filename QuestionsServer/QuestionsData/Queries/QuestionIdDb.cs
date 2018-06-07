using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class QuestionIdDb
  {
    [Key]
    public int QuestionId { get; set; }
  }
}