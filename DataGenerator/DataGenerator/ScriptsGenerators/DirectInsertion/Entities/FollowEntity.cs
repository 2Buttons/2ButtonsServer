using System;

namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{
  public class FollowEntity
  {
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public int VisitsCount { get; set; }
    public DateTime FollowedDate { get; set; }
  }
}