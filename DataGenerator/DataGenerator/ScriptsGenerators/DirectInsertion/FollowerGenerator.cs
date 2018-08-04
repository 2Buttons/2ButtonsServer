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

    private string SwitchIdentityInsert(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Followers]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return "INSERT INTO [dbo].[Followers] ([followerID], [followingID], [visitsCount], [followedDate], [isDeleted]) VALUES" +
             Environment.NewLine;
    }

    private string GetInsertionFollowLine(FollowEntity follow)
    {
      return $"({follow.FollowerId}, {follow.FollowingId}, {follow.VisitsCount}, '{follow.FollowedDate:u}', 0)";
    }

    private string GetInsertionFollowsLine(IList<FollowEntity> follows)
    {
      var result = new StringBuilder();
      foreach (var entity in follows)
      {
        result.Append(GetInsertInit() + GetInsertionFollowLine(entity) + GetGo());
      }
      return result.ToString();
    }

    public string GetInsertionLine(IList<FollowEntity> follows)
    {
      var result = new StringBuilder();
      var times = follows.Count < 1000 ? 1 : follows.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var cfollowsIter = follows.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        //result.Append("ALTER TABLE [dbo].[Follow] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        //result.Append(GetGo());
       // result.Append(SwitchIdentityInsert(true));
       // result.Append(GetGo());
        //result.Append(GetInsertInit());
        result.Append(GetInsertionFollowsLine(cfollowsIter));
        result.Append(GetGo());
      //  result.Append(SwitchIdentityInsert(false));
       // result.Append(GetGo());
        //result.Append("ALTER TABLE [dbo].[Follow] NOCHECK CONSTRAINT FK_OPTION_QUESTION");
        //result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}