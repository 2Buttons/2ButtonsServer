using System;

namespace QuestionsData.Queries.NewsQuestions
{
    public class NewsFavoriteQuestionsDb : NewsQuestionBaseDb
  {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}