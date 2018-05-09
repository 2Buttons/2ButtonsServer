using System;

namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class UserQuestionsViewModel : UserPageIdViewModel
  {
    public SortType SortType { get; set; } = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class PersonalQuestionsViewModel : UserIdViewModel
  {
    public SortType SortType = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class TopDayQuestions : UserIdViewModel
  {
    public DateTime TopAfterDate { get; set; } = DateTime.UtcNow.AddDays(-1);
    public bool IsOnlyNew { get; set; } = true;
    public SortType SortType { get; set; } = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class TopAllQuestions : UserIdViewModel
  {
    public bool IsOnlyNew { get; set; } = true;
    public SortType SortType { get; set; } = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }
}
