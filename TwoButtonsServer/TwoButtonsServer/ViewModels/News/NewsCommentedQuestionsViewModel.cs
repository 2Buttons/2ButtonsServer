using System;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsCommentedQuestionsViewModel : QuestionBaseViewModel
    {
        public int? CommentUserId { get; set; }
        public string CommentUserLogin { get; set; }
        public int? CommentAmount { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}