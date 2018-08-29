using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Followings")]
  public class FollowingEntity
  {
    public int UserId { get; set; }
    public int FollowingId { get; set; }
    public int VisitsCount { get; set; }
    public bool IsFollowing { get; set; }
    public DateTime FollowingDate { get; set; }
    public DateTime LastVisitDate { get; set; }
  }
}