using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Tags")]
  public class TagEntity
  {
    [Key]
    public int TagId { get; set; }

    public string Text { get; set; }
  }
}