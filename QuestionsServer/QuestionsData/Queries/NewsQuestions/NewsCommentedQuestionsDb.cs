using System;

namespace QuestionsData.Queries.NewsQuestions
{
    public class NewsCommentedQuestionsDb : NewsQuestionBaseDb
  {
        public int CommentUserId { get; set; }
        public string CommentUserLogin { get; set; }
        public int CommentsAmount { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}