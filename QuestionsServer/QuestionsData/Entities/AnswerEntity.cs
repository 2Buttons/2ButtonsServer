using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Entities
{
  [Table("Anwser")]
  public class AnswerEntity
  {
    [Key]
    public int UserId { get; set; }

    public int QuestionId { get; set; }

    [Column("anwser")]
    public AnswerType AnswerType { get; set; }

    public int Liked { get; set; }
    public DateTime AnswerDate { get; set; }

    [Column("deleted")]
    public int IsDeleted { get; set; }
  }
}