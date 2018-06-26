using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("Tag")]
  public class TagEntity
  {
    [Key]
    public int TagId { get; set; }

    public string TagText { get; set; }
  }
}