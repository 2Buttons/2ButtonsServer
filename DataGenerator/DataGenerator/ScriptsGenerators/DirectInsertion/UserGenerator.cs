using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGenerator.ScriptsGenerators.DirectInsertion
{
  public class UserGenerator
  {
    private readonly string _db = "TwoButtons";
    private readonly string _dbAccount = "TwoButtonsAccount";

    private string GetUsingLineMainDb()
    {
      return $"USE [{_db}]";
    }

    private string GetUsingLineAccountDb()
    {
      return $"USE [{_dbAccount}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string SwitchIdentityInsertAccount(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Users]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string SwitchIdentityInsertMain(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Users]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertionInitLineMain()
    {
      return "INSERT INTO[dbo].[Users] " + "([userID]," + "[firstName], " + "[lastName], " + "[birthDate], " + "[sexType], " + "[cityID], " +
             "[description], " + "[lastNotsSeenDate], " + "[OriginalAvatarUrl])" + "VALUES" +
             Environment.NewLine;
    }

    private string GetInserionInitLineAccount()
    {
      return "INSERT INTO [dbo].[Users] " + " ([userID], [AccessFailedCount]," + "[Email]," + "[EmailConfirmed]," +
             "[PasswordHash]," + "[PhoneNumber]," + "[PhoneNumberConfirmed]," + "[RoleType]," + "[TwoFactorEnabled]," +
             "[RegistrationDate]," + "[IsBot])" + "VALUES" + Environment.NewLine;
    }

    private string GetInserionLineAccount(UserEntity user)
    {
      return
        $"({user.UserId}, 0, N'{user.Email}', 1, N'{user.PasswordHash}', '{user.PhoneNumber}', 1, {(int) user.RoleType}, 0, '{user.RegistrationDate:u}', 1)";
    }

    private string GetInserionLineMain(UserInfoEntity user)
    {
      return
        $"({user.UserId}, N'{user.FirstName}', N'{user.LastName}', '{user.BirthDate:u}', {(int) user.SexType}, {user.CityId}, N'{user.Description}', '{user.LastNotsSeenDate:u}', N'{user.OriginalAvatarUrl}')";
    }

    private string GetInsertionLineUsersMain(IList<UserInfoEntity> users)
    {
      var result = new StringBuilder();
      for (var i = 0; i < users.Count - 1; i++)
        result.Append(GetInserionLineMain(users[i]) + "," + Environment.NewLine);
      result.Append(GetInserionLineMain(users[users.Count - 1]));
      return result.ToString();
    }

    private string GetInsertionLineUsersAccount(IList<UserEntity> users)
    {
      var result = new StringBuilder();
      for (var i = 0; i < users.Count - 1; i++)
        result.Append(GetInserionLineAccount(users[i]) + "," + Environment.NewLine);
      result.Append(GetInserionLineAccount(users[users.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLineAccount(IList<UserEntity> users)
    {
      var result = new StringBuilder();
      var times = users.Count < 1000 ? 1 : users.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var usersIter = users.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingLineAccountDb());
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertAccount(true));
        result.Append(GetGo());
        result.Append(GetInserionInitLineAccount());
        result.Append(GetInsertionLineUsersAccount(usersIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertAccount(false));
        result.Append(GetGo());
      }
      return result.ToString();
    }


    private string GetInserionLineAccountAddUser(UserInfoEntityAddUSer user)
    {
      return
        $"EXECUTE [dbo].[addUser] {user.UserId}, N'{user.FirstName}', N'{user.LastName}', '{user.BirthDate:u}', { (int)user.SexType}, { user.City}, N'{user.Description}',  N'{user.OriginalAvatarUrl}')";
    }

    private string GetInsertionLineUsersAccountAddUser(IList<UserInfoEntityAddUSer> users)
    {
      var result = new StringBuilder();
      for (var i = 0; i < users.Count - 1; i++)
        result.Append(GetInserionLineAccountAddUser(users[i]) + "," + Environment.NewLine);
      result.Append(GetInserionLineAccountAddUser(users[users.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLineAccountAddUser(IList<UserInfoEntityAddUSer> users)
    {
      var result = new StringBuilder();
      var times = users.Count < 1000 ? 1 : users.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var usersIter = users.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingLineAccountDb());
        result.Append(GetGo());
        result.Append(GetInsertionLineUsersAccountAddUser(usersIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }

    public string GetInsertionLineMain(IList<UserInfoEntity> users)
    {
      var result = new StringBuilder();
      var times = users.Count < 1000 ? 1 : users.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var usersIter = users.Skip(i * 1000).Take(1000).ToList();
        
        result.Append(GetUsingLineMainDb());
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Users] NOCHECK CONSTRAINT FK_USER_CITY");
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertMain(true));
        result.Append(GetGo());
        result.Append(GetInsertionInitLineMain());
        result.Append(GetInsertionLineUsersMain(usersIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertMain(false));
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Users] WITH CHECK CHECK CONSTRAINT FK_USER_CITY");
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}