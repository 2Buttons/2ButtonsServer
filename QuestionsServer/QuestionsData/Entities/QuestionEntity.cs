using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Entities
{
  [Table("Questions")]
  public class QuestionEntity
  {
    [Key]
    public long QuestionId { get; set; }

    public long UserId { get; set; }
    public string Condition { get; set; }
    public bool IsAnonymous { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime AddedDate { get; set; }
    public string OriginalBackgroundUrl{ get; set; }
    public int AnswersCount { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public bool IsDeleted { get; set; }
  }
}