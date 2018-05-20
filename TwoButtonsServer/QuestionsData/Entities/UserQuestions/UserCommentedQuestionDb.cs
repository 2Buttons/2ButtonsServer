using System;

namespace QuestionsData.Entities.UserQuestions
{
    public partial class UserCommentedQuestionDb : QuestionBaseDb
    {
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public int CommentLikes { get; set; }
        public int CommentDislikes { get; set; }
        public int YourCommentFeedback { get; set; }
        public int? PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}