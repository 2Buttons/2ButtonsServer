﻿using System;
using System.Collections.Generic;
using System.Linq;
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
      return "INSERT INTO [dbo].[Users] " + " ([userID], [AccessFailedCount]," + "[Email]," + "[EmailConfirmed]," +
             "[PasswordHash]," + "[PhoneNumber]," + "[PhoneNumberConfirmed]," + "[RoleType]," + "[TwoFactorEnabled]," +
             "[RegistrationDate]," + "[IsBot])" + "VALUES" + Environment.NewLine;
    }

    private string GetInserionLineAccount(UserEntity user)
    {
      return
        $"({user.UserId}, 0, N'{user.Email}', 1, N'{user.PasswordHash}', '{user.PhoneNumber}', 1, {(int) user.RoleType}, 0, '{user.RegistrationDate}', 1)";
    }

    private string GetInserionLineMain(UserInfoEntity user)
    {
      return
        $"({user.UserId}, N'{user.Login}', N'{user.BirthDate}', {(int) user.SexType}, {user.CityId}, N'{user.Description}', N'{user.LastNotsSeenDate}', N'{user.LargeAvatarLink}', N'{user.SmallAvatarLink}')";
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

    public string GetInsertionLineMain(IList<UserInfoEntity> users)
    {
      var result = new StringBuilder();
      var times = users.Count < 1000 ? 1 : users.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var usersIter = users.Skip(i * 1000).Take(1000).ToList();
        
        result.Append(GetUsingLineMainDb());
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[User] NOCHECK CONSTRAINT FK_USER_CITY");
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertMain(true));
        result.Append(GetGo());
        result.Append(GetInsertionInitLineMain());
        result.Append(GetInsertionLineUsersMain(usersIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsertMain(false));
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT FK_USER_CITY");
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}