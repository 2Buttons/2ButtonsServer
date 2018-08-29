using System;
using CommonLibraries;

namespace SocialData.Main.Queries
{
  public class FollowingQuery
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public int VisitsCount { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
  }
}