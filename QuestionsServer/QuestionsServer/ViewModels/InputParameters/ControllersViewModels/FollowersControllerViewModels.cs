namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class FollowerViewModel : UserPageIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
        public string SearchedLogin { get; set; } = "";
    }

  public class FollowViewModel
  {
    public int FollowerId { get; set; }
    public int FollowToId { get; set; }
  }
}
