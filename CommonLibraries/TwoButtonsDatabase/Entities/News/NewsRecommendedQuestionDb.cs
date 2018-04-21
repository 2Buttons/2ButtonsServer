using System;

namespace TwoButtonsDatabase.Entities.News
{
    public class NewsRecommendedQuestionDb : QuestionBaseDb
    {
        public int RecommendedUserId { get; set; }
        public string RecommendedUserLogin { get; set; }
        public DateTime RecommendedDate { get; set; }
    }
}