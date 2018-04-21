using System;

namespace TwoButtonsDatabase.Entities.NewsQuestions
{
    public class NewsCommentedQuestionsDb : QuestionBaseDb
    {
        public int? CommentUserId { get; set; }
        public string CommentUserLogin { get; set; }
        public int? CommentAmount { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}