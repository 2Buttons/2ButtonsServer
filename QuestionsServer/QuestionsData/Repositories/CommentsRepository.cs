using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;

namespace QuestionsData.Repositories
{
  public class CommentsRepository
  {
    private readonly TwoButtonsContext _db;

    public CommentsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<int> AddComment(int userId, int questionId, string comment, int? previousCommentId)
    {
      var commentedTime = DateTime.UtcNow;

      var commentedId = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};

       _db.Database.ExecuteSqlCommand(
        $"addComment {userId}, {questionId}, {comment}, {commentedTime}, {previousCommentId}, {commentedId} OUT");
      return (int) commentedId.Value;
    }

    public async Task<bool> UpdateCommentFeedback(int userId, int commentId, QuestionFeedbackType feedback)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateCommentFeedback {userId}, {commentId}, {feedback}") > 0;
    }

    public async Task<List<CommentDb>> GetComments(int userId, int questionId, int count)
    {
      return await _db.CommentDb.AsNoTracking()
               .FromSql($"select * from dbo.getComments({userId}, {questionId}, {1}, {count})").ToListAsync() ??
             new List<CommentDb>();
    }
  }
}