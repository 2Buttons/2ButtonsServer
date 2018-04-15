using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class CommentsWrapper
    {
        public static bool TryAddComment(TwoButtonsContext db, int userId, int questionId, String comment, int previousCommentId)
        {
          

                var userIdDb = new SqlParameter("@userID", userId);
                var questionIdDb = new SqlParameter("@questionId", questionId);
                var commentDb = new SqlParameter("@comment", comment);
                var previousCommentIdDb = new SqlParameter("@previousCommentId", previousCommentId);

                var commentedTime = DateTime.Now;
                var commentedTimeDb = new SqlParameter("@commented", commentedTime);

                try
                {
                    db.Database.ExecuteSqlCommand("addComment @userID, @questionId, @comment, @previousCommentId, @commented",
                        userIdDb,
                        questionIdDb,
                        commentDb,
                        previousCommentIdDb,
                        commentedTimeDb);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            
        }

        public static bool TryAddCommentFeedback(TwoButtonsContext db, int userId, int commentId, int feedback)
        {
            
                var userIdDb = new SqlParameter("@userID", userId);
                var commentIdDb = new SqlParameter("@commentId", commentId);
                var feedbackDb = new SqlParameter("@feedback", feedback);

                try
                {
                    db.Database.ExecuteSqlCommand("addCommentFeedback @userID, @commentId, @feedback",
                        userIdDb,
                        commentIdDb,
                        feedbackDb);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            
        }

        public static bool TryGetComments(TwoButtonsContext db, int userId, int questionId, int amount, out IEnumerable<CommentDb> comments)
        {


           

                try
                {

                    comments = db.CommentDb.FromSql($"select * from dbo.getComments({userId}, {questionId}, {amount})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                comments = new List<CommentDb>();
                return false;
            

            //// throw new NotImplementedException();
            // using (var db = new TwoButtonsContext())
            // {

            //     var userIdDb = new SqlParameter("@userID", userId);
            //     var commentIdDb = new SqlParameter("@questionId", questionId);
            //     var feedbackDb = new SqlParameter("@amount", amount);

            //     try
            //     {
            //         db.Database.ExecuteSqlCommand("select * from dbo.getComments(?, ?, ?) @userID, @commentId, @feedback, @comments OUT",
            //             userIdDb,
            //             commentIdDb,
            //             feedbackDb);
            //         return true;
            //     }
            //     catch (Exception e)
            //     {
            //         Console.WriteLine(e);
            //     }
            //     return false;
            // }

            /*
              // throw new NotImplementedException();
            using (var db = new TwoButtonsContext())
            {

                var userIdDb = new SqlParameter("@userID", userId);
                var commentIdDb = new SqlParameter("@questionId", questionId);
                var amountDb = new SqlParameter("@amount", amount);

                try
                {

                    db.Database.ExecuteSqlCommand($"select * from dbo.getComments({userId}, {questionId}, {amount})");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                comments = new List<ReturnedComment>();
                return false;
            }
            */
        }


    }
}
