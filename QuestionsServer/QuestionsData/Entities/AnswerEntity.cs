using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Entities
{
  [Table("Answers")]
  public class AnswerEntity
  {
    [Key]
    public int UserId { get; set; }

    public int QuestionId { get; set; }
    public AnswerType AnswerType { get; set; }
    public bool IsLiked { get; set; }
    public DateTime AnsweredDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}