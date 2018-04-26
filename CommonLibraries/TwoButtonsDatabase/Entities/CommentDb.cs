using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class CommentDb
    {
        [Key]
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public string Comment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int? YourFeedback { get; set; }
        public int? PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}