using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("QuestionTags")]
  public class QuestionTagEntity
  {
    [Key]
    public long QuestionId { get; set; }
    public int TagId { get; set; }
  }
}