using System;

namespace TwoButtonsDatabase.Entities.News
{
    public class NewsFavouriteQuestionsDb : QuestionBaseDb
    {
        public int FavoriteAddedUserId { get; set; }
        public string FavoriteAddedUserLogin { get; set; }
        public DateTime FavoriteAddDate { get; set; }
    }
}