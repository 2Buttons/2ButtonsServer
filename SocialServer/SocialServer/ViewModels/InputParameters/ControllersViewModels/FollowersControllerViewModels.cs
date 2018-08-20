using CommonLibraries.Validation;

namespace SocialServer.ViewModels.InputParameters.ControllersViewModels
{
  public class FollowingViewModel : UserPageIdViewModel
  {
    public string SearchedLogin { get; set; }
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class FollowViewModel
  {
    [IdValidation]
    public int UserId { get; set; }
    [IdValidation]
    public int FollowingId { get; set; }
  }
}