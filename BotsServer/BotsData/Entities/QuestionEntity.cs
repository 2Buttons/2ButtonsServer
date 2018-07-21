using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace BotsData.Entities
{
  [Table("Question")]
  public class QuestionEntity
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserId { get; set; }
    public string Condition { get; set; }

    [Column("anonymity")]
    public QuestionType QuestionType { get; set; }

    [Column("audience")]
    public AudienceType AudienceType { get; set; }

    public DateTime QuestionAddDate { get; set; }

    public string OriginalBackgroundUrl{ get; set; }

    public int Likes { get; set; }
    public int Dislikes { get; set; }

    [Column("deleted")]
    public bool IsDeleted { get; set; }
  }
}