using System;

namespace QuestionsData.Entities.NewsQuestions
{
    public class NewsFavoriteQuestionsDb : NewsQuestionBaseDb
  {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}