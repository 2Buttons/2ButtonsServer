using CommonLibraries;

namespace SocialServer.ViewModels.OutputParameters
{
  public class GetFollowerViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarUrl { get; set; }
    public int Age { get; set; }
    public SexType SexType { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
  }
}
