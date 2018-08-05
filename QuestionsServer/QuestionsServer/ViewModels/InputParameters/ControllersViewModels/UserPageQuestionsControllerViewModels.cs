using CommonLibraries;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class UserQuestionsViewModel : UserPageIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    public SortType SortType { get; set; } = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class PersonalQuestionsViewModel : UserIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    public SortType SortType = SortType.DateTime;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class TopDayQuestions : UserIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    public int DeltaUnixTime { get; set; } = 24 * 60 * 60;
    public bool IsOnlyNew { get; set; } = true;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  //public class TopAllQuestions : UserIdViewModel
  //{
  //  public bool IsOnlyNew { get; set; } = true;
  //  public SortType SortType { get; set; } = SortType.DateTime;
  //  public PageParams PageParams { get; set; } = new PageParams();
  //}
}
