using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO;
using QuestionsData.DTO.NewsQuestions;
using QuestionsData.Queries.UserQuestions;

namespace QuestionsData.Repositories
{
  public class NewsQuestionsRepository
  {

    public List<NewsAskedQuestionDto> GetNewsAskedQuestions(TwoButtonsContext context, long userId)
    {
      return context.NewsAskedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getNewsAskedQuestions({userId})")
        .Select(x => new NewsAskedQuestionDto { NewsAskedQuestionDb = x, Priority = x.AnsweredFollowingsCount * 4 }).OrderBy(x => x.NewsAskedQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsAnsweredQuestionDto> GetNewsAnsweredQuestions(TwoButtonsContext context, long userId)
    {
      return context.NewsAnsweredQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsAnsweredQuestions({userId})")
        .Select(x => new NewsAnsweredQuestionDto { NewsAnsweredQuestionDb = x, Priority = x.AnsweredFollowingsCount }).OrderBy(x => x.NewsAnsweredQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsFavoriteQuestionDto> GetNewsFavoriteQuestions(TwoButtonsContext context, long userId)
    {
      return context.NewsFavoriteQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsFavoriteQuestions({userId})")
        .Select(x => new NewsFavoriteQuestionDto { NewsFavoriteQuestionDb = x, Priority = x.AnsweredFollowingsCount }).OrderBy(x => x.NewsFavoriteQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsCommentedQuestionDto> GetNewsCommentedQuestions(TwoButtonsContext context, long userId)
    {
      return context.NewsCommentedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsCommentedQuestions({userId})")
        .Select(x => new NewsCommentedQuestionDto
        {
          NewsCommentedQuestionDb = x,
          Priority = x.CommentsCount * x.AnsweredFollowingsCount * 2
        }).OrderBy(x => x.NewsCommentedQuestionDb.UserId).ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsRecommendedQuestionDto> GetNewsRecommendedQuestions(TwoButtonsContext context, long userId)
    {
      var questions = context.NewsRecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})").ToList();


        //.Select(x => new NewsRecommendedQuestionDto { NewsRecommendedQuestionDb = x, Priority = x.AnsweredFollowTo * 7 }).OrderBy(x => x.NewsRecommendedQuestionDb.UserId)
        //.ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();



      var result = new List<NewsRecommendedQuestionDto>();

      foreach (var t in questions)
      {
        if (result.Count == 0 || result.All(x => x.QuestionId != t.QuestionId))
        {
          result.Add(new NewsRecommendedQuestionDto
          {
            QuestionId = t.QuestionId,
            Condition = t.Condition,
            FirstOption = t.FirstOption,
            SecondOption = t.SecondOption,
            OriginalBackgroundUrl = t.OriginalBackgroundUrl,
            QuestionType = t.QuestionType,
            AddedDate = t.AddedDate,
            UserId = t.UserId,
            Login = t.Login,
            OriginalAvatarUrl = t.OriginalAvatarUrl,
            LikesCount = t.LikesCount,
            DislikesCount = t.DislikesCount,
            YourFeedbackType = t.YourFeedbackType,
            YourAnswerType = t.YourAnswerType,
            IsInFavorites = t.IsInFavorites,
            IsSaved = t.IsSaved,
            CommentsCount = t.CommentsCount,
            FirstAnswersCount = t.FirstAnswersCount,
            SecondAnswersCount = t.SecondAnswersCount,

            Priority = t.AnsweredFollowingsCount * 7,

            RecommendedUsers = new List<RecommendedUserDto>()
          });

        }
        var question = result.FirstOrDefault(x => x.QuestionId == t.QuestionId);
        if (question != null && question.RecommendedUsers.All(x => x.UserId != t.RecommendedUserId))
        {
          question.RecommendedUsers
          .Add(new RecommendedUserDto { UserId = t.RecommendedUserId, Login = t.RecommendedUserLogin, SexType = t.RecommendedUserSexType });
        }
      }

      return result.OrderBy(x => x.UserId)
        .ThenByDescending(x => x.Priority).ToList();
    }

    /*
    public List<NewsRecommendedQuestionDto> GetNewsRecommendedQuestions(TwoButtonsContext context, long userId)
    {
      var  questions =  context.NewsRecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})").ToListAsync();
        .Select(x => new NewsRecommendedQuestionDto { NewsRecommendedQuestionDb = x, Priority = x.AnsweredFollowTo * 7}).OrderBy(x => x.NewsRecommendedQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();

    

      var result = new List<NewsRecommendedQuestionDto>();

      foreach (NewsRecommendedQuestionDto t in questions)
      {
        if (result.Count == 0 || result.All(x => x..QuestionId != t.QuestionId))
        {
          result.Add(new NewsRecommendedQuestionDto
          {
            QuestionId = t.QuestionId,
            Condition = t.Condition,
            FirstOption = t.FirstOption,
            SecondOption = t.SecondOption,
            BackgroundImageUrl = t.BackgroundImageUrl,
            QuestionType = t.QuestionType,
            QuestionAddDate = t.QuestionAddDate,
            UserId = t.UserId,
            Login = t.Login,
            SmallAvatarUrl = t.SmallAvatarUrl,
            Likes = t.Likes,
            Dislikes = t.Dislikes,
            YourFeedback = t.YourFeedback,
            YourAnswer = t.YourAnswer,
            InFavorites = t.InFavorites,
            IsSaved = t.IsSaved,
            Comments = t.Comments,
            FirstAnswers = t.FirstAnswers,
            SecondAnswers = t.SecondAnswers,
          });

        }
        var question = result.FirstOrDefault(x => x.QuestionId == t.QuestionId);
        if (question != null && question.RecommendedToUsers.All(x => x.UserId != t.ToUserId)) question.RecommendedToUsers
          .Add(new RecommendedToUserDto { UserId = t.ToUserId, Login = t.ToUserLogin });
      }

      return result;
    } 
  */
  }
}