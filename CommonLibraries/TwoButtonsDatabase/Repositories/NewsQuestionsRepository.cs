using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.NewsQuestions;

namespace TwoButtonsDatabase.Repositories
{
  public class NewsQuestionsRepository
  {
    private readonly TwoButtonsContext _db;

    public NewsQuestionsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public bool TryGetNewsAskedQuestions(int userId, int offset, int count,
      out IEnumerable<NewsAskedQuestionsDb> newsAskedQuestions)
    {
      try
      {
        newsAskedQuestions = _db.NewsAskedQuestionsDb
          .FromSql($"select * from dbo.getNewsAskedQuestions({userId})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newsAskedQuestions = new List<NewsAskedQuestionsDb>();
      return false;
    }

    public bool TryGetNewsAnsweredQuestions(int userId, int offset, int count,
      out IEnumerable<NewsAnsweredQuestionsDb> newsAnsweredQuestions)
    {
      try
      {
        newsAnsweredQuestions = _db.NewsAnsweredQuestionsDb
          .FromSql($"select * from dbo.getNewsAnsweredQuestions({userId})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newsAnsweredQuestions = new List<NewsAnsweredQuestionsDb>();
      return false;
    }

    public bool TryGetNewsFavoriteQuestions(int userId, int offset, int count,
      out IEnumerable<NewsFavoriteQuestionsDb> newsFavoriteQuestions)
    {
      try
      {
        newsFavoriteQuestions = _db.NewsFavoriteQuestionsDb
          .FromSql($"select * from dbo.getNewsFavoriteQuestions({userId})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newsFavoriteQuestions = new List<NewsFavoriteQuestionsDb>();
      return false;
    }

    public bool TryGetNewsCommentedQuestions(int userId, int offset, int count,
      out IEnumerable<NewsCommentedQuestionsDb> newsCommentedQuestions)
    {
      try
      {
        newsCommentedQuestions = _db.NewsCommentedQuestionsDb
          .FromSql($"select * from dbo.getNewsCommentedQuestions({userId})")
          .Skip(offset).Take(count)
          .ToList();

        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newsCommentedQuestions = new List<NewsCommentedQuestionsDb>();
      return false;
    }

    public bool TryGetNewsRecommendedQuestions(int userId, int offset, int count,
      out IEnumerable<NewsRecommendedQuestionDb> newRecommendedQuestion)
    {
      try
      {
        newRecommendedQuestion = _db.NewsRecommendedQuestionDb
          .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      newRecommendedQuestion = new List<NewsRecommendedQuestionDb>();
      return false;
    }
  }
}