using System;

namespace TwoButtonsServer.ViewModels.OutputParameters.UserQuestions
{
    public class UserCommentedQuestionsViewModel : QuestionBaseViewModel
    {
        public int? CommentId { get; set; }
        public string Comment { get; set; }
        public int? CommentLikes { get; set; }
        public int? CommentDislikes { get; set; }
        public int? YourCommentFeedback { get; set; }
        public int? PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}