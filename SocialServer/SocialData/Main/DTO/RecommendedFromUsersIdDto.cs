using CommonLibraries;
using System;

namespace SocialData.Main.DTO
{
  public class RecommendedFromUsersIdDto
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
  }
}