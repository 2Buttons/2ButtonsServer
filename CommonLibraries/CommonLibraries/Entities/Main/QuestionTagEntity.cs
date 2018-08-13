using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("QuestionTags")]
  public class QuestionTagEntity
  {
    [Key]
    public int QuestionId { get; set; }
    public int TagId { get; set; }
  }
}