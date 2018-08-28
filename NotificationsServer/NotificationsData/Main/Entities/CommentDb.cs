using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationsData.Main.Entities
{

  [Table("Comments")]
  public class CommentDb
  {
    [Key]
    public int CommentId { get; set; }

    public int UserId { get; set; }
    public string Text { get; set; }
    public int PreviousCommentId { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CommentedDate { get; set; }
  }
}