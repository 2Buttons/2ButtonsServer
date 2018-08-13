using System;
using CommonLibraries;

namespace SocialData.Main.DTO
{
  public class FollowingDto
  {
    public int UserId { get; set; }

    public string Login { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
    public int VisitsCount { get; set; }
  }
}