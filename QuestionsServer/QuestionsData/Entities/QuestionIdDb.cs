using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Entities
{
  public class QuestionIdDb
  {
    [Key]
    public int QuestionId { get; set; }
  }
}