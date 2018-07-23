using System;

namespace DataGenerator.ScriptsGenerators.Entities
{
  public class FollowEntity
  {
    public int UserId { get; set; }
    public int FollowingId { get; set; }
    public int VisitsCount { get; set; }
    public DateTime FollowedDate { get; set; }
  }
}