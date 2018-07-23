using System;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarUrl { get; set; }
        public string Text { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public QuestionFeedbackType YourFeedbackType { get; set; }
        public int PreviousCommentId { get; set; }
        public DateTime AddDate { get; set; }
    }
}