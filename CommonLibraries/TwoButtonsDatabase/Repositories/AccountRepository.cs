using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.Account;

namespace TwoButtonsDatabase.Repositories
{
  public class AccountRepository
  {
    private readonly TwoButtonsContext _db;

    public AccountRepository(TwoButtonsContext db)
    {
      _db = db;
    }


    public bool TryAddUser(int userId, string login, DateTime birthDate, SexType sex,
      string city, string description, string fullAvatarLink, string smallAvatarLink)
    {
      var returnsCode = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };

      try
      {
        _db.Database.ExecuteSqlCommand(
          $"addUser {userId}, {login}, {birthDate}, {sex}, {city},  {description}, {fullAvatarLink}, {smallAvatarLink}, {returnsCode} OUT");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    //public  bool TryGetIdentification( string login, string password, out int userId)
    //{
    //  try
    //  {
    //    userId = _db.IdentificationDb
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

    //public  bool TryCheckValidUser( string phone, string login, out int returnsCode)
    //{
    //  try
    //  {
    //    returnsCode = _db.CheckValidUserDb
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

    //public  bool TryIsUserIdValid( int userId, out bool isValid)
    //{
    //  try
    //  {
    //    isValid = (_db.IsUserIdValidDb
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

    //public  bool TryGetUserRole( int userId, out int role)
    //{
    //  try
    //  {
    //    role = _db.RoleDb.FromSql($"select * from dbo.getRole({userId})").FirstOrDefault()?.Role ?? 1;
    //    return true;
    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine(e);
    //  }
    //  role = 1;
    //  return false;
    //}

    public bool TryUpdateUserFullAvatar(int userId, string fullAvatarLink)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"updateUserFullAvatar {userId}, {fullAvatarLink}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryUpdateUserSmallAvatar( int userId, string smallAvatar)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"updateUserSmallAvatar {userId}, {smallAvatar}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryGetUserInfo( int userId, int getUserId, out UserInfoDb userInfo)
    {
      try
      {
        userInfo = _db.UserInfoDb.FromSql($"select * from dbo.getUserInfo({userId}, {getUserId})").FirstOrDefault() ??
                   new UserInfoDb();

        if (userId != getUserId)
          if (userInfo.YouFollowed)
            TryUpdateVisits(userId, getUserId);
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      userInfo = new UserInfoDb();
      return false;
    }

    public bool TryGetUserStatistics( int userId, out UserStatisticsDb userStatistics)
    {
      try
      {
        userStatistics = _db.UserStatisticsDb
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

    public bool TryUpdateVisits( int userId, int getUserId)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"updateVisits {userId}, {getUserId}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryGetUserRaiting( int userId, out int rainting)
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