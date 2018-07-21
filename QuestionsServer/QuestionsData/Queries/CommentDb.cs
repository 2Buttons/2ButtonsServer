using System;
using CommonLibraries;

namespace QuestionsData.Queries
{
    public partial class CommentDb
    {
      
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarUrl { get; set; }
        public string Comment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public QuestionFeedbackType YourFeedback { get; set; }
        public int? PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}