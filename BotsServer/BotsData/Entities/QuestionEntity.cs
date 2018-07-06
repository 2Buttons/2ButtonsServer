using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotsData
{
  [Table("Question")]
  public class QuestionEntity
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserId { get; set; }
    public string Condition { get; set; }

    [Column("anonymity")]
    public int IsAnonymity { get; set; }

    [Column("audience")]
    public int IsAudience { get; set; }

    public int QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }

    [Column("backbroundImageLink")]
    public string BackgroundImageLink { get; set; }

    public int Answers { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }

    [Column("deleted")]
    public int IsDeleted { get; set; }
  }
}