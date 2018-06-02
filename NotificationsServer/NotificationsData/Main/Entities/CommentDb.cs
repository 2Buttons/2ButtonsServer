using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationsData.Main.Entities
{
  public class CommentDb
  {
    [Key]
    public int CommentId { get; set; }

    public int UserId { get; set; }
    public string Comment { get; set; }
    public int? PreviousCommentId { get; set; }
    public int? Deelted { get; set; }
    public DateTime CommentAddDate { get; set; }
  }
}