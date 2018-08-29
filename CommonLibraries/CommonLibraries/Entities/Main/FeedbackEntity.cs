using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Feedbacks")]
  public class FeedbackEntity
  {
    [Key]
    public int FeedbackId { get; set; }

    public int UserId { get; set; }
    public FeedbackType FeedbackType { get; set; }
    public string Text { get; set; }
    public DateTime AddedDate { get; set; }
  }
}