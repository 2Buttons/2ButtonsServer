using System;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsCommentedQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int CommentUserId { get; set; }
        public string CommentUserLogin { get; set; }
        public int UserCommentsAmount { get; set; }
    }
}