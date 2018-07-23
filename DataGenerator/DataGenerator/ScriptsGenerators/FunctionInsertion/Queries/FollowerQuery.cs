using System;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class FollowerQuery
  {
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime FollowedDate { get; set; }
  }
}