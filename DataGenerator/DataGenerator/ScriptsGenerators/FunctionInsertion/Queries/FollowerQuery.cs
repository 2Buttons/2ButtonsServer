using System;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class UserRelationshipQuery
  {
    public int UserId { get; set; }
    public int StaredUserId { get; set; }
    public DateTime FollowingDate { get; set; }
  }
}