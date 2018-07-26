using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators
{
  public class UserGenerator
  {
    private readonly string _db = "TwoButtonsAccount";

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string GetInsertionUserLine(UserQuery user)
    {
      var isEmailConfirmed = user.IsEmailConfirmed ? 1 : 0;
      var isPhoneConfirmed = user.IsPhoneNumberConfirmed ? 1 : 0;
      var isBot = user.IsBot ? 1 : 0;
      var isTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled ? 1 : 0;

      return
        $"EXECUTE [dbo].[addUserWithId] {user.UserId}, N'{user.Email}', {isEmailConfirmed}, N'{user.PasswordHash}', N'{user.PhoneNumber}', {isPhoneConfirmed}, {(int)user.RoleType}, {isTwoFactorAuthenticationEnabled}, {isBot}, '{user.RegistrationDate:u}'";
    }

    private string GetInsertionUsersLine(IList<UserQuery> users)
    {
      var result = new StringBuilder();
      for (var i = 0; i < users.Count - 1; i++)
        result.Append(GetInsertionUserLine(users[i]) + ";" + Environment.NewLine);
      result.Append(GetInsertionUserLine(users[users.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<UserQuery> users)
    {
      var result = new StringBuilder();
      var times = users.Count < 1000 ? 1 : users.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var usersIter = users.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionUsersLine(usersIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}