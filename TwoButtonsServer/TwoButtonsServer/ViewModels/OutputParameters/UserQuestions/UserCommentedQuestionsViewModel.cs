using System;
using CommonLibraries;

namespace TwoButtonsServer.ViewModels.OutputParameters.UserQuestions
{
    public class UserCommentedQuestionsViewModel : QuestionBaseViewModel
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int CommentLikesAmount { get; set; }
        public int CommentDislikesAmount { get; set; }
        public FeedbackType YourCommentFeedbackType { get; set; }
        public int PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}