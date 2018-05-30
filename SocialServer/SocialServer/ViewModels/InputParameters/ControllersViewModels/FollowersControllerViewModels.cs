using CommonLibraries.Validation;

namespace SocialServer.ViewModels.InputParameters.ControllersViewModels
{
  public class FollowerViewModel : UserPageIdViewModel
  {
    public PageParams PageParams { get; set; } = new PageParams();
    public string SearchedLogin { get; set; } = "";
  }

  public class FollowViewModel
  {
    [IdValidation]
    public int UserId { get; set; }
    [IdValidation]
    public int FollowToId { get; set; }
  }
}