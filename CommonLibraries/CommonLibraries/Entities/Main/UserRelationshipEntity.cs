using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("UserRelationships")]
  public class UserRelationshipEntity
  {
    [Key]
    public int UserId { get; set; }
    public int StaredUserId { get; set; }
    public int VisitsCount { get; set; }
    public bool IsFollowing { get; set; }
    public DateTime FollowingDate { get; set; }
    public DateTime LastVisitDate { get; set; }
  }
}