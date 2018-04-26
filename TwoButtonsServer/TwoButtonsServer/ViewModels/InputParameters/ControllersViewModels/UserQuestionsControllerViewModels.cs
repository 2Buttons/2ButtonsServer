using System;

namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class UserQuestionsViewModel: UserPageIdViewModel
    {
        public int QuestionsAmount { get; set; } = 100;
    }

    public class PersonalQuestionsViewModel : UserIdViewModel
    {
        public int QuestionsAmount { get; set; } = 100;
    }

    public class TopUserQuestions : UserIdViewModel
    {
        public DateTime TopAfterDate { get; set; } = DateTime.UtcNow;
        public bool IsOnlyNew { get; set; } = true;
        public int QuestionsAmount { get; set; } = 100;
    }
}
