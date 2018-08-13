using System;

namespace CommonLibraries.Entities.Main
{
  public class UserRelationshipEntity
  {
    public int UserId { get; set; }
    public int StaredUserId { get; set; }
    public int VisitsCount { get; set; }
    public bool IsFollowing { get; set; }
    public DateTime FollowingDate { get; set; }
    public DateTime LastVisitDate { get; set; }
  }
}