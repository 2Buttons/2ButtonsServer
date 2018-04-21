using System;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsAnsweredQuestionsViewModel : QuestionBaseViewModel
    {
        public int AnsweredFollowTo { get; set; } /*amount of answered followsTo*/
        public DateTime AnswerDate { get; set; }
    }
}