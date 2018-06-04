using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AccountData.Main.Entities
{
  [Table("Feedbacks")]
  public class FeedbackDb
  {
    [Key]
    public int FeedbackId { get; set; }

    public int UserId { get; set; }
    public FeedbackType Type { get; set; }
    public string Text { get; set; }
  }
}