using System;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsAnsweredQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int AnsweredFollowTo { get; set; } /*amount of answered followsTo*/
        public DateTime AnswerDate { get; set; }
    }
}