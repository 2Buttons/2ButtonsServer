using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace BotsData.Entities
{
  [Table("Questions")]
  public class QuestionEntity
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserId { get; set; }
    public string Condition { get; set; }
    public QuestionType QuestionType { get; set; }
    public AudienceType AudienceType { get; set; }
    public DateTime AddedDate { get; set; }
    public string OriginalBackgroundUrl{ get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public bool IsDeleted { get; set; }
  }
}