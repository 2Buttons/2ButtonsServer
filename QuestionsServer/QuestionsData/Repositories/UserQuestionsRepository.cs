﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO;
using QuestionsData.DTO.NewsQuestions;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsData.Queries.UserQuestions;

namespace QuestionsData.Repositories
{
  public class UserQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public UserQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddRecommendedQuestion(int userToId, int userFromId, int questionId)
    {
      if (_db.UserEntities.All(x => x.UserId != userToId)) return false;

      var recommendedDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"addRecommendedQuestion {userToId}, {userFromId}, {questionId}, {recommendedDate}") > 0;
    }

    public async Task<List<UserAskedQuestionDb>> GetUserAskedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserAskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserAskedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserAnsweredQuestionDb>> GetUserAnsweredQuestions(int userId, int pageUserId, int offset,
      int count)
    {
      var isAnonimus = userId == pageUserId ? 1 : 0;

      return await _db.UserAnsweredQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(x => x.QuestionAddDate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserFavoriteQuestionDb>> GetUserFavoriteQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserFavoriteQuestionDb, object>> predicate)
    {
      var isAnonimus = 0;

      return await _db.UserFavoriteQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<UserCommentedQuestionDto>> GetUserCommentedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<UserCommentedQuestionDb, object>> predicate)
    {
      var questions =  await _db.UserCommentedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId})").OrderByDescending(predicate)
        .Skip(offset).Take(count).ToListAsync();



      var result = new List<UserCommentedQuestionDto>();

      foreach (var t in questions)
      {
        if (result.Count == 0 || result.All(x => x.QuestionId != t.QuestionId))
        {
          result.Add(new UserCommentedQuestionDto
          {
            QuestionId = t.QuestionId,
            Condition = t.Condition,
            FirstOption = t.FirstOption,
            SecondOption = t.SecondOption,
            BackgroundImageLink = t.BackgroundImageLink,
            QuestionType = t.QuestionType,
            QuestionAddDate = t.QuestionAddDate,
            UserId = t.UserId,
            Login = t.Login,
            SmallAvatarLink = t.SmallAvatarLink,
            LikesCount = t.LikesCount,
            DislikesCount = t.DislikesCount,
            YourFeedback = t.YourFeedback,
            YourAnswer = t.YourAnswer,
            InFavorites = t.InFavorites,
            IsSaved = t.IsSaved,
            CommentsCount = t.CommentsCount,
            FirstAnswersCount = t.FirstAnswersCount,
            SecondAnswersCount = t.SecondAnswersCount,

            Comments = new List<UserCommentQuestionDto>()
          });

        }
        var question = result.FirstOrDefault(x => x.QuestionId == t.QuestionId);
        if (question != null && question.Comments.All(x => x.CommentId != t.CommentId))
        {
          question.Comments
            .Add(new  UserCommentQuestionDto {  CommentId = t.CommentId, AddDate = t.CommentAddDate, DislikesAmount = t.CommentDislikes, LikesAmount = t.CommentLikes, PreviousCommentId = t.PreviousCommentId,  YourFeedbackType = (FeedbackType)t.YourCommentFeedback, Text = t.Text});
        }
      }

      return result;


    }

    public async Task<List<TopQuestionDb>> GeTopQuestions(int userId, bool isOnlyNew, int offset, int count,
      DateTime topAfterDate, Expression<Func<TopQuestionDb, object>> predicate)
    {
      return await _db.TopQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getTop({userId}, {topAfterDate}, {isOnlyNew})").OrderByDescending(predicate)
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<AskedQuestionDb>> GetAskedQuestions(int userId, int pageUserId, int offset, int count,
      Expression<Func<AskedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.AskedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId},   {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<RecommendedQuestionDb>> GetRecommendedQuestions(int userId, int pageUserId, int offset,
      int count, Expression<Func<RecommendedQuestionDb, object>> predicate)
    {
      return await _db.RecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserRecommendedQuestions({userId})").OrderByDescending(predicate).Skip(offset)
        .Take(count).ToListAsync();
    }

    public async Task<List<RecommendedQuestionDto>> GetRecommendedQuestions(int userId,int offset,
      int count, Expression<Func<RecommendedQuestionDb, object>> predicate)
    {

      var questions = await _db.RecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserRecommendedQuestions({userId})").OrderByDescending(predicate).ToListAsync();

      var result = new List<RecommendedQuestionDto>();

      foreach (RecommendedQuestionDb t in questions)
      {
        if (result.Count == 0 || result.All(x=>x.QuestionId != t.QuestionId))
        { result.Add(new RecommendedQuestionDto
          {
            QuestionId = t.QuestionId,
            Condition = t.Condition,
            FirstOption = t.FirstOption,
            SecondOption = t.SecondOption,
            BackgroundImageLink = t.BackgroundImageLink,
            QuestionType = t.QuestionType,
            QuestionAddDate = t.QuestionAddDate,
            UserId = t.UserId,
            Login = t.Login,
            SmallAvatarLink = t.SmallAvatarLink,
            LikesCount = t.LikesCount,
            DislikesCount = t.DislikesCount,
            YourFeedback = t.YourFeedback,
            YourAnswer = t.YourAnswer,
            InFavorites = t.InFavorites,
            IsSaved = t.IsSaved,
            CommentsCount = t.CommentsCount,
            FirstAnswersCount = t.FirstAnswersCount,
            SecondAnswersCount = t.SecondAnswersCount,
          });
       
        }
        var question = result.FirstOrDefault(x => x.QuestionId == t.QuestionId);
        if(question!=null && question.RecommendedToUsers.All(x=>x.UserId != t.ToUserId)) question.RecommendedToUsers
          .Add(new RecommendedToUserDto {UserId = t.ToUserId, Login = t.ToUserLogin});
      }

      return result;

     
    }

    public async Task<List<SelectedQuestionDb>> GetSelectedQuestions(int userId, int pageUserId, int offset, int count)
    {
      return await _db.SelectedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNotAnsweredQuestions({userId})")
        .OrderByDescending(x => (5 * x.LikesCount - 3 * x.DislikesCount > 10
                                  ? 5 * x.LikesCount - 3 * x.DislikesCount
                                  : 15 + 5 * x.LikesCount - 3 * x.DislikesCount) - (x.FirstAnswersCount + x.SecondAnswersCount))
        .Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<LikedQuestionDb>> GetLikedQuestions(int userId, int offset, int count,
      Expression<Func<LikedQuestionDb, object>> predicate)
    {
      return await _db.LikedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getUserLikedQuestions({userId})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<SavedQuestionDb>> GetSavedQuestions(int userId, int offset, int count,
      Expression<Func<SavedQuestionDb, object>> predicate)
    {
      var isAnonimus = 1;

      return await _db.SavedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {userId}, {isAnonimus})")
        .OrderByDescending(predicate).Skip(offset).Take(count).ToListAsync();
    }
  }
}