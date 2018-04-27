using System;

namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
    public class NewsAnsweredQuestionViewModel : NewsQuestionBaseViewModel
    {
        public int AnsweredFollowToAmount { get; set; } /*amount of answered followsTo*/
        public DateTime AnswerDate { get; set; }
    }
}