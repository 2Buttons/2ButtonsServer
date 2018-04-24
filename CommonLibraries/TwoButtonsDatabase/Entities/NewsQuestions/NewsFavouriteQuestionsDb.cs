using System;

namespace TwoButtonsDatabase.Entities.NewsQuestions
{
    public class NewsFavoriteQuestionsDb : QuestionBaseDb
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}