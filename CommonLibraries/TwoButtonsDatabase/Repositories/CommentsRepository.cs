using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.Repositories
{
  public class CommentsRepository
  {
    private readonly TwoButtonsContext _db;

    public CommentsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public bool TryAddComment(int userId, int questionId, string comment,
      int previousCommentId, out int commentId)
    {
      var commentedTime = DateTime.UtcNow;

      var ommentedIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };


      try
      {
        _db.Database.ExecuteSqlCommand(
          $"addComment {userId}, {questionId}, {comment}, {previousCommentId}, {commentedTime}, {ommentedIdDb} OUT");
        commentId = (int) ommentedIdDb.Value;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      commentId = -1;
      return false;
    }

    public bool TryUpdateCommentFeedback(int userId, int commentId, FeedbackType feedback)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"updateCommentFeedback {userId}, {commentId}, {feedback}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public bool TryGetComments(int userId, int questionId, int count,
      out IEnumerable<CommentDb> comments)
    {
      try
      {
        comments = _db.CommentDb.FromSql($"select * from dbo.getComments({userId}, {questionId}, {1}, {count})")
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