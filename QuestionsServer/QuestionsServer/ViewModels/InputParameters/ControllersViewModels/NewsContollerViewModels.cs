using CommonLibraries;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class GetNewsViewModel : UserIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    public PageParams PageParams { get; set; } = new PageParams();
  }
}
