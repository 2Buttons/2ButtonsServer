﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class CommentsWrapper
    {
        public static bool TryAddComment(TwoButtonsContext db, int userId, int questionId, string comment, int previousCommentId, out int commentId)
        {
            var commentedTime = DateTime.UtcNow;

            var ommentedIdDb = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };


            try
            {
                db.Database.ExecuteSqlCommand($"addComment {userId}, {questionId}, {comment}, {previousCommentId}, {commentedTime}, {ommentedIdDb} OUT");
                commentId = (int)ommentedIdDb.Value;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            commentId = -1;
            return false;
        }

        public static bool TryUpdateCommentFeedback(TwoButtonsContext db, int userId, int commentId, FeedbackType feedback)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"updateCommentFeedback {userId}, {commentId}, {feedback}");
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
                comments = db.CommentDb.FromSql($"select * from dbo.getComments({userId}, {questionId}, {1}, {amount})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            comments = new List<CommentDb>();
            return false;
        }
    }
}