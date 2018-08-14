using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;

namespace DataGenerator.ScriptsGenerators.DirectInsertion
{
  public class FollowerGenerator
  {
    private readonly string _db = "TwoButtons";

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[Followings] ([UserID], [FollowingID], [visitsCount], [IsFollowing], [FollowingDate], [LastVisitDate]) VALUES" +
             Environment.NewLine;
    }

    private string GetInsertionFollowLine(FollowingEntity follow)
    {
      var isFollowing = 1;// follow.IsFollowing ? 1 : 0;
      return $"({follow.UserId}, {follow.FollowingId}, {follow.VisitsCount}, {isFollowing}, '{follow.FollowingDate:u}', '{follow.LastVisitDate:u}')";
    }

    private string GetInsertionFollowsLine(IList<FollowingEntity> follows)
    {
      var result = new StringBuilder();
      foreach (var entity in follows)
      {
        result.Append(GetInsertInit() + GetInsertionFollowLine(entity) + GetGo());
      }
      return result.ToString();
    }

    public string GetInsertionLine(IList<FollowingEntity> follows)
    {
      var result = new StringBuilder();
      var times = follows.Count < 1000 ? 1 : follows.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var cfollowsIter = follows.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionFollowsLine(cfollowsIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}