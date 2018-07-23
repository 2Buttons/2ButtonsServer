using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class QuestionIdQuery
  {
    [Key]
    public long QuestionId { get; set; }
  }
}