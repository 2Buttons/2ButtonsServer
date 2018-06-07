using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<int> AddComment(int userId, int questionId, string comment,
      int previousCommentId)
    {
      var commentedTime = DateTime.UtcNow;

      var ommentedIdDb = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };


      try
      {
        await _db.Database.ExecuteSqlCommandAsync(
          $"addComment {userId}, {questionId}, {comment}, {previousCommentId}, {commentedTime}, {ommentedIdDb} OUT");
        return (int) ommentedIdDb.Value;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return -1;
    }

    public async Task<bool> UpdateCommentFeedback(int userId, int commentId, FeedbackType feedback)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"updateCommentFeedback {userId}, {commentId}, {feedback}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<List<CommentDb>> GetComments(int userId, int questionId, int count)
    {
      try
      {
        return await _db.CommentDb.AsNoTracking().FromSql($"select * from dbo.getComments({userId}, {questionId}, {1}, {count})")
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<CommentDb>();
    }
  }
}