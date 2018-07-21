using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("Tags")]
  public class TagEntity
  {
    [Key]
    public int TagId { get; set; }

    public string Text { get; set; }
  }
}