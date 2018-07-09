using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace BotsData.Entities
{
  [Table("Anwser")]
  public class AnswerEntity
  {
    [Key]
    public int UserId { get; set; }

    public int QuestionId { get; set; }
    [Column("Anwser")]
    public AnswerType AnswerType { get; set; }
  }
}