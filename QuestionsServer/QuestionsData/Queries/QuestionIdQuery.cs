using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class QuestionIdQuery
  {
    [Key]
    public int QuestionId { get; set; }
  }
}