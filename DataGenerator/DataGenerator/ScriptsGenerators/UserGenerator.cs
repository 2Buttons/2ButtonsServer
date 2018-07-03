using System;
using System.Collections.Generic;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[User]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertionInitLineMain()
    {
      return "INSERT INTO[dbo].[User] " + "([userID]," + "[login], " + "[birthDate], " + "[sex], " + "[cityID], " +
             "[description], " + "[lastNotsSeenDate], " + "[fullAvatarLink], " + "[smallAvatarLink])" + "VALUES" +
             Environment.NewLine;
    }

    private string GetInserionInitLineAccount()
    {
      return "INSERT INTO [dbo].[Users] " + " ([AccessFailedCount]," + "[Email]," +
             "[EmailConfirmed]," + "[PasswordHash]," + "[PhoneNumber]," + "[PhoneNumberConfirme]," +
             "[RoleType]," + "[TwoFactorEnabled]," + "[RegistrationDate]," + "[IsBot])" +
             "VALUES" + Environment.NewLine;
    }

    private string GetInserionLineAccount(UserEntity user)
    {
      return
        $"({user.UserId}, 0, '{user.Email}', 1, {user.PasswordHash}, {user.PhoneNumber}, 1, {user.RoleType}, 0, '{user.RegistrationDate}', 1)";
    }
    private string GetInserionLineMain(UserInfoEntity user)
    {
      return
        $"({user.UserId}, {user.Login}, '{user.BirthDate}', {(int)user.SexType}, {user.CityId}, {user.Description}, '{user.LastNotsSeenDate}', '{user.LargeAvatarLink}', '{user.SmallAvatarLink}')";
    }

    private string GetInsertionLineUsersMain(IList<UserInfoEntity> users )
    {
      StringBuilder result = new StringBuilder();
      for (int i = 0; i < users.Count - 1; i++)
      {
        result.Append(GetInserionLineMain(users[i]) + "," + Environment.NewLine);
      }
      result.Append(GetInserionLineMain(users[users.Count - 1]));
      return result.ToString();
    }

    private string GetInsertionLineUsersAccount(IList<UserEntity> users)
    {
      StringBuilder result = new StringBuilder();
      for (int i = 0; i < users.Count - 1; i++)
      {
        result.Append(GetInserionLineAccount(users[i]) + "," + Environment.NewLine);
      }
      result.Append(GetInserionLineAccount(users[users.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLineAccount(IList<UserEntity> users)
    {
      StringBuilder result = new StringBuilder();
      result.Append(GetUsingLineAccountDb());
      result.Append(GetGo());
      result.Append(SwitchIdentityInsertAccount(true));
      result.Append(GetGo());
      result.Append(GetInserionInitLineAccount());
      result.Append(GetInsertionLineUsersAccount(users));
      result.Append(GetGo());
      result.Append(SwitchIdentityInsertAccount(false));
      result.Append(GetGo());
      return result.ToString();
    }

    public string GetInsertionLineMain(IList<UserInfoEntity> users)
    {
      StringBuilder result = new StringBuilder();
      result.Append(GetUsingLineMainDb());
      result.Append(GetGo());
      result.Append(SwitchIdentityInsertMain(true));
      result.Append(GetGo());
      result.Append(GetInsertionInitLineMain());
      result.Append(GetInsertionLineUsersMain(users));
      result.Append(GetGo());
      result.Append(SwitchIdentityInsertMain(false));
      result.Append(GetGo());
      return result.ToString();
    }
  }
}