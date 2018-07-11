using System;
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

    [Column("anwserDate")]
    public DateTime AnsweredDate { get; set; }

    [Column("deleted")]
    public int IsDeleted { get; set; }

    [Column("liked")]
    public int IsLiked { get; set; }
  }
}