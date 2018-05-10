using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Account;


namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class AccountWrapper
  {

    //private readonly TwoButtonsContext _db;

    //public UserWrapper(TwoButtonsContext db)
    //{
    //  _db = db;
    //}


    public static bool TryAddUser(TwoButtonsContext db, int userId, string login, int age, SexType sex, string city, string description, string fullAvatarLink, string smallAvatarLink)
    {

      var userIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output,
      };

      var registrationDate = DateTime.UtcNow;

      try
      {
        db.Database.ExecuteSqlCommand($"addUser {login}, {""}, {age}, {sex}, {city}, {""}, {description}, {fullAvatarLink}, {smallAvatarLink}, {1}, {registrationDate}, {userIdDb} OUT");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;

    }

    //public static bool TryGetIdentification(TwoButtonsContext db, string login, string password, out int userId)
    //{
    //  try
    //  {
    //    userId = db.IdentificationDb
    //               .FromSql($"select * from dbo.identification({login}, {password})").FirstOrDefault()
    //               ?.UserId ?? -1;
    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  userId = -1;
    //  return false;
    //}

    //public static bool TryCheckValidUser(TwoButtonsContext db, string phone, string login, out int returnsCode)
    //{
    //  try
    //  {
    //    returnsCode = db.CheckValidUserDb
    //                    .FromSql($"select * from dbo.checkValidUser({phone}, {login})").FirstOrDefault()
    //                    ?.ReturnCode ?? -1;
    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  returnsCode = -1;
    //  return false;
    //}

    //public static bool TryIsUserIdValid(TwoButtonsContext db, int userId, out bool isValid)
    //{
    //  try
    //  {
    //    isValid = (db.IsUserIdValidDb
    //                 .FromSql($"select * from dbo.isUserIdValid({userId})").FirstOrDefault()
    //                 ?.IsValid ?? false);
    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  isValid = false;
    //  return false;
    //}

    //public static bool TryGetUserRole(TwoButtonsContext db, int userId, out int role)
    //{
    //  try
    //  {
    //    role = db.RoleDb.FromSql($"select * from dbo.getRole({userId})").FirstOrDefault()?.Role ?? 1;
    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  role = 1;
    //  return false;
    //}

    public static bool TryUpdateUserFullAvatar(TwoButtonsContext db, int userId, string fullAvatarLink)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"updateUserFullAvatar {userId}, {fullAvatarLink}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryUpdateUserSmallAvatar(TwoButtonsContext db, int userId, string smallAvatar)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"updateUserSmallAvatar {userId}, {smallAvatar}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryGetUserInfo(TwoButtonsContext db, int userId, int getUserId, out UserInfoDb userInfo)
    {
      try
      {
        userInfo = db.UserInfoDb.FromSql($"select * from dbo.getUserInfo({userId}, {getUserId})").FirstOrDefault() ?? new UserInfoDb();

        if (userId != getUserId)
          if (userInfo.YouFollowed == 1)
            TryUpdateVisits(db, userId, getUserId);
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userInfo = new UserInfoDb();
      return false;
    }

    public static bool TryGetUserStatistics(TwoButtonsContext db, int userId, out UserStatisticsDb userStatistics)
    {
      try
      {
        userStatistics = db.UserStatisticsDb
                       .FromSql($"select * from dbo.getUserStatistics({userId})").FirstOrDefault() ??
                   new UserStatisticsDb();

        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userStatistics = new UserStatisticsDb();
      return false;
    }

    //public static bool TryGetUserContacts(TwoButtonsContext db, int userId, out List<UserContactsDb> userContacts)
    //{
    //  try
    //  {
    //    userContacts = db.UserContactsDb
    //                       .FromSql($"select * from dbo.getUserContacts({userId})").ToList();

    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  userContacts = new List<UserContactsDb>();
    //  return false;
    //}

    public static bool TryUpdateVisits(TwoButtonsContext db, int userId, int getUserId)
    {
      try
      {
        db.Database.ExecuteSqlCommand($"updateVisits {userId}, {getUserId}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public static bool TryGetUserRaiting(TwoButtonsContext db, int userId, out int rainting)
    {
      rainting = -100500;
      return false;

      //ResultSet rsQuestions;
      //ResultSet rsComments;
      //PreparedStatement stQuestions;
      //PreparedStatement stComments;

      //int questionsRaiting = 0;
      //int commentsRaiting = 0;

      //try
      //{
      //    stQuestions = con.prepareStatement("select dbo.getUserQuestionsRaiting(?)");
      //    stQuestions.setInt(1, userId);
      //    rsQuestions = stQuestions.executeQuery();

      //    if (rsQuestions.next())
      //        questionsRaiting = rsQuestions.getInt(1);

      //    stComments = con.prepareStatement("select dbo.getUserCommentsRaiting(?)");
      //    stComments.setInt(1, userId);
      //    rsComments = stComments.executeQuery();

      //    if (rsComments.next())
      //        commentsRaiting = rsComments.getInt(1);
      //}
      //catch (SQLException e)
      //{
      //    e.printStackTrace();
      //}

      //int raiting = questionsRaiting + commentsRaiting;

      //return raiting;
    }
  }
}