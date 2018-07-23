using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class QuestionIdDb
  {
    [Key]
    public long QuestionId { get; set; }
  }
}