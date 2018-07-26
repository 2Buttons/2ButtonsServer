using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators
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

    private string GetInsertionFollowerLine(FollowerQuery follower)
    {
      return
        $"EXECUTE [dbo].[addFollow] {follower.FollowerId}, {follower.FollowingId}, '{follower.FollowedDate:u}'";
    }

    private string GetInsertionFollowersLine(IList<FollowerQuery> followers)
    {
      var result = new StringBuilder();
      for (var i = 0; i < followers.Count - 1; i++)
        result.Append(GetInsertionFollowerLine(followers[i]) + ";" + Environment.NewLine);
      result.Append(GetInsertionFollowerLine(followers[followers.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<FollowerQuery> followers)
    {
      var result = new StringBuilder();
      var times = followers.Count < 1000 ? 1 : followers.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var followersIter = followers.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionFollowersLine(followersIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}