using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestionsData.DTO.NewsQuestions;

namespace QuestionsData.Repositories
{
  public class NewsQuestionsRepository
  {

    public List<NewsAskedQuestionDto> GetNewsAskedQuestions(TwoButtonsContext context, int userId)
    {
      return context.NewsAskedQuestionsDb.AsNoTracking().FromSql($"select * from dbo.getNewsAskedQuestions({userId})")
        .Select(x => new NewsAskedQuestionDto {NewsAskedQuestionDb = x, Priority = x.AnsweredFollowTo * 4}).OrderBy(x => x.NewsAskedQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsAnsweredQuestionDto> GetNewsAnsweredQuestions(TwoButtonsContext context, int userId)
    {
      return context.NewsAnsweredQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsAnsweredQuestions({userId})")
        .Select(x => new NewsAnsweredQuestionDto {NewsAnsweredQuestionDb = x, Priority = x.AnsweredFollowTo}).OrderBy(x => x.NewsAnsweredQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsFavoriteQuestionDto> GetNewsFavoriteQuestions(TwoButtonsContext context, int userId)
    {
      return context.NewsFavoriteQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsFavoriteQuestions({userId})")
        .Select(x => new NewsFavoriteQuestionDto {NewsFavoriteQuestionDb = x, Priority = x.AnsweredFollowTo}).OrderBy(x => x.NewsFavoriteQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsCommentedQuestionDto> GetNewsCommentedQuestions(TwoButtonsContext context, int userId)
    {
      return context.NewsCommentedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsCommentedQuestions({userId})")
        .Select(x => new NewsCommentedQuestionDto
        {
          NewsCommentedQuestionDb = x,
          Priority = x.Comments * x.AnsweredFollowTo * 2
        }).OrderBy(x=>x.NewsCommentedQuestionDb.UserId).ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }

    public List<NewsRecommendedQuestionDto> GetNewsRecommendedQuestions(TwoButtonsContext context, int userId)
    {
      return context.NewsRecommendedQuestionsDb.AsNoTracking()
        .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})")
        .Select(x => new NewsRecommendedQuestionDto {NewsRecommendedQuestionDb = x, Priority = x.AnsweredFollowTo * 7}).OrderBy(x => x.NewsRecommendedQuestionDb.UserId)
        .ThenByDescending(x => x.Priority).ToListAsync().GetAwaiter().GetResult();
    }
  }
}