﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;
using QuestionsData.Queries;

namespace QuestionsData.Repositories
{
  public class CommentsRepository
  {
    private readonly TwoButtonsContext _db;

    public CommentsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<int> AddComment(long userId, long questionId, string comment, int? previousCommentId)
    {
      var commentedTime = DateTime.UtcNow;

      var commentedId = new SqlParameter {SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};

       await _db.Database.ExecuteSqlCommandAsync(
        $"addComment {userId}, {questionId}, {comment}, {previousCommentId}, {commentedTime}, {commentedId} OUT");
      return (int) commentedId.Value;
    }

    public async Task<bool> UpdateCommentFeedback(long userId, int commentId, QuestionFeedbackType feedback)
    {
      return await _db.Database.ExecuteSqlCommandAsync($"updateCommentFeedback {userId}, {commentId}, {feedback}") > 0;
    }

    public async Task<List<CommentDb>> GetComments(long userId, long questionId, int offset, int count)
    {
      return await _db.CommentDb.AsNoTracking()
               .FromSql($"select * from dbo.getComments({userId}, {questionId})").OrderByDescending(x=>x.CommentedDate).Skip(offset).Take(count).ToListAsync() ??
             new List<CommentDb>();
    }
  }
}