using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class UserPageWrapper
    {

        public static bool TryGetUserInfo(TwoButtonsContext db, int userId, int getUserId, out UserInfoDb userInfo)
        {
            
                try
                {
                    var p = db.UserInfoDb
                        .FromSql($"select * from dbo.getUserInfo({userId}, {getUserId})");
                    var t = p;
                    userInfo = db.UserInfoDb
                        .FromSql($"select * from dbo.getUserInfo({userId}, {getUserId})").FirstOrDefault() ?? new UserInfoDb();

                    if (userId != getUserId)
                    {

                        if (userInfo.YouFollowed.HasValue && userInfo.YouFollowed == 1)
                        {
                            TryUpdateVisits(db, userId, getUserId);
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userInfo = new UserInfoDb();
                return false;
            
           
        }

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
            throw new NotImplementedException();

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

        public static bool TryGetPosts(TwoButtonsContext db, int userId, int getUserId, int amount, out IEnumerable<PostDb> posts)
        {
            
                try
                {
                    posts = db.PostDb
                                   .FromSql($"select * from dbo.getPosts({userId}, {getUserId}, {amount})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                posts = new List<PostDb>();
                return false;
            
        }
    }
}
