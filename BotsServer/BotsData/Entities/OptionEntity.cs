using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotsData.Entities
{
  [Table("Option")]
  public class OptionEntity
  {
    [Key]
    public int OptionId { get; set; }

    public int QuestionId { get; set; }
    public string OptionText { get; set; }
    public int Position { get; set; }
    public int Answers { get; set; }
  }
}