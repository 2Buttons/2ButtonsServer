using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion
{
  public class UserInfoGenerator
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

    private string GetInsertionUserInfoLine(UserInfoQuery userInfo)
    {
      return
        $"EXECUTE [dbo].[addUser] {userInfo.UserId}, N'{userInfo.Login}', '{userInfo.BirthDate:u}', {(int)userInfo.SexType}, N'{userInfo.City}', N'{userInfo.Description}', N'{userInfo.OriginalAvatarUrl}'";
    }

    private string GetInsertionUserInfosLine(IList<UserInfoQuery> userInfos)
    {
      var result = new StringBuilder();
      for (var i = 0; i < userInfos.Count - 1; i++)
        result.Append(GetInsertionUserInfoLine(userInfos[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionUserInfoLine(userInfos[userInfos.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<UserInfoQuery> userInfos)
    {
      var result = new StringBuilder();
      var times = userInfos.Count < 1000 ? 1 : userInfos.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var userInfosIter = userInfos.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionUserInfosLine(userInfosIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}