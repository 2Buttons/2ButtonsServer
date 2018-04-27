using System;

namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class UserQuestionsViewModel: UserPageIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
    }

    public class PersonalQuestionsViewModel : UserIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
    }

    public class TopUserQuestions : UserIdViewModel
    {
        public DateTime TopAfterDate { get; set; } = DateTime.UtcNow.AddDays(-1);
        public bool IsOnlyNew { get; set; } = true;
        public PageParams PageParams { get; set; } = new PageParams();
    }
}
