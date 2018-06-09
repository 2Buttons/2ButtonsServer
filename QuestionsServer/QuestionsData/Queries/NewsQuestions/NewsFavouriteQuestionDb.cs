using System;

namespace QuestionsData.Queries.NewsQuestions
{
    public class NewsFavoriteQuestionDb : NewsQuestionBaseDb
  {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
    }
}