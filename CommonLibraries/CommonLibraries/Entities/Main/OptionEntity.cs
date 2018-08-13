using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Options")]
  public class OptionEntity
  {
    [Key]
    public int OptionId { get; set; }

    public int QuestionId { get; set; }
    public string Text { get; set; }
    public int Position { get; set; }
    public int AnswersCount { get; set; }
  }
}