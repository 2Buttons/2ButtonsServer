using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Answers")]
  public class AnswerEntity
  {
    [Key]
    public int UserId { get; set; }

    public int QuestionId { get; set; }
    public AnswerType AnswerType { get; set; }
    public QuestionFeedbackType FeedbackType { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsSaved { get; set; }
    public DateTime AnsweredDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}