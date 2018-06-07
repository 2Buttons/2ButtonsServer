using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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


    public async Task<bool> AddUser(int userId, string login, DateTime birthDate, SexType sex,
      string city, string description, string fullAvatarLink, string smallAvatarLink)
    {
      var returnsCode = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };

      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"addUser {userId}, {login}, {birthDate}, {sex}, {city},  {description}, {fullAvatarLink}, {smallAvatarLink}, {returnsCode} OUT") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    //public  bool GetIdentification( string login, string password, out int userId)
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

    //public  bool CheckValidUser( string phone, string login, out int returnsCode)
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

    //public  bool IsUserIdValid( int userId, out bool isValid)
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

    //public  bool GetUserRole( int userId, out int role)
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

    public async Task<bool> UpdateUserFullAvatar(int userId, string fullAvatarLink)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateUserFullAvatar {userId}, {fullAvatarLink}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<bool> UpdateUserSmallAvatar(int userId, string smallAvatar)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateUserSmallAvatar {userId}, {smallAvatar}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<UserInfoDb> GetUserInfo(int userId, int userPageId)
    {
      try
      {
        var user = await _db.UserInfoDb.AsNoTracking().FromSql($"select * from dbo.getUserInfo({userId}, {userPageId})")
                     .FirstOrDefaultAsync() ??
                   new UserInfoDb();

        if (userId != userPageId)
          if (user.YouFollowed)
            UpdateVisits(userId, userPageId);
        return user;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new UserInfoDb();
    }

    public async Task<UserStatisticsDb> GetUserStatistics(int userId)
    {
      try
      {
        return await _db.UserStatisticsDb
                 .FromSql($"select * from dbo.getUserStatistics({userId})").FirstOrDefaultAsync() ??
               new UserStatisticsDb();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new UserStatisticsDb();
    }

    public async Task<bool> UpdateVisits(int userId, int getUserId)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateVisits {userId}, {getUserId}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
    //  //PreparedStatement stComments;
    //  //PreparedStatement stQuestions;
    //  //ResultSet rsComments;

    //  //ResultSet rsQuestions;
    //  return false;
    //  rainting = -100500;
    //{

    //public async Task<bool> GetUserRaiting( int userId, out int rainting)

    //  //int questionsRaiting = 0;
    //  //int commentsRaiting = 0;

    //  //try
    //  //{
    //  //    stQuestions = con.prepareStatement("select dbo.getUserQuestionsRaiting(?)");
    //  //    stQuestions.setInt(1, userId);
    //  //    rsQuestions = stQuestions.executeQuery();

    //  //    if (rsQuestions.next())
    //  //        questionsRaiting = rsQuestions.getInt(1);

    //  //    stComments = con.prepareStatement("select dbo.getUserCommentsRaiting(?)");
    //  //    stComments.setInt(1, userId);
    //  //    rsComments = stComments.executeQuery();

    //  //    if (rsComments.next())
    //  //        commentsRaiting = rsComments.getInt(1);
    //  //}
    //  //catch (SQLException e)
    //  //{
    //  //    e.printStackTrace();
    //  //}

    //  //int raiting = questionsRaiting + commentsRaiting;

    //  //return raiting;
    //}
  }
}