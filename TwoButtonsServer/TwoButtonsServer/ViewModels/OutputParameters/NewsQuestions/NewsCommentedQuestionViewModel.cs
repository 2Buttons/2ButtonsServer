using System;


namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsCommentedQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int? CommentUserId { get; set; }
        public string CommentUserLogin { get; set; }
        public int? CommentsAmount { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}