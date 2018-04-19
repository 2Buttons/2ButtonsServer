using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class UserWrapper
    {

        public static bool TryAddUser(TwoButtonsContext db, string login, string password, int age, int sex, string phone, string description, string fullAvatarLink, string smallAvatarLink, out int userId)
        {

            var userIdDb = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            var regDate = DateTime.Now;

            try
            {
                db.Database.ExecuteSqlCommand($"addUser {login}, {password}, {age}, {sex}, {phone}, {description}, {fullAvatarLink}, {smallAvatarLink},{regDate}, {userIdDb} OUT");
                userId = (int)userIdDb.Value;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userId = -1;
            return false;

        }

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
                userInfo = db.UserInfoDb
                               .FromSql($"select * from dbo.getUserInfo({userId}, {getUserId})").FirstOrDefault() ??
                           new UserInfoDb();

                if (userId != getUserId)
                    if (userInfo.YouFollowed.HasValue && userInfo.YouFollowed == 1)
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

        public static bool TryGetPosts(TwoButtonsContext db, int userId, int getUserId, int amount,
            out IEnumerable<PostDb> posts)
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