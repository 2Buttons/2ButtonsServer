using System;

namespace TwoButtonsDatabase.Entities.News
{
    public partial class NewsAnsweredQuestionsDb : QuestionBaseDb
    {
        public int AnsweredFollowTo { get; set; } /*amount of answered followsTo*/
        public DateTime AnswerDate { get; set; }
    }
}