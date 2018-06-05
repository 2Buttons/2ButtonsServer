using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationsData.Main.Entities
{

  [Table("Comment")]
  public class CommentDb
  {
    [Key]
    public int CommentId { get; set; }

    public int UserId { get; set; }
    public string Comment { get; set; }
    public int? PreviousCommentId { get; set; }
    public int? Deleted { get; set; }
    public DateTime CommentAddDate { get; set; }
  }
}