using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.NewsQuestions;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class NewsQuestionsWrapper
  {
    public static bool TryGetNewsAskedQuestions(TwoButtonsContext db, int userId, int page, int amount,
      out IEnumerable<NewsAskedQuestionsDb> newsAskedQuestions)
    {
      var fromLine = page * amount - amount;

      try
      {
        newsAskedQuestions = db.NewsAskedQuestionsDb
          .FromSql($"select * from dbo.getNewsAskedQuestions({userId})")
          .Skip(fromLine).Take(amount)
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

    public static bool TryGetNewsAnsweredQuestions(TwoButtonsContext db, int userId, int page, int amount,
      out IEnumerable<NewsAnsweredQuestionsDb> newsAnsweredQuestions)
    {
      var fromLine = page * amount - amount;

      try
      {
        newsAnsweredQuestions = db.NewsAnsweredQuestionsDb
          .FromSql($"select * from dbo.getNewsAnsweredQuestions({userId})")
          .Skip(fromLine).Take(amount)
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

    public static bool TryGetNewsFavoriteQuestions(TwoButtonsContext db, int userId, int page, int amount,
      out IEnumerable<NewsFavoriteQuestionsDb> newsFavoriteQuestions)
    {
      var fromLine = page * amount - amount;

      try
      {
        newsFavoriteQuestions = db.NewsFavoriteQuestionsDb
          .FromSql($"select * from dbo.getNewsFavoriteQuestions({userId})")
          .Skip(fromLine).Take(amount)
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

    public static bool TryGetNewsCommentedQuestions(TwoButtonsContext db, int userId, int page, int amount,
      out IEnumerable<NewsCommentedQuestionsDb> newsCommentedQuestions)
    {
      var fromLine = page * amount - amount;

      try
      {
        newsCommentedQuestions = db.NewsCommentedQuestionsDb
          .FromSql($"select * from dbo.getNewsCommentedQuestions({userId})")
          .Skip(fromLine).Take(amount)
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

    public static bool TryGetNewsRecommendedQuestions(TwoButtonsContext db, int userId, int page, int amount,
      out IEnumerable<NewsRecommendedQuestionDb> newRecommendedQuestion)
    {
      var fromLine = page * amount - amount;

      try
      {
        newRecommendedQuestion = db.NewsRecommendedQuestionDb
          .FromSql($"select * from dbo.getNewsRecommendedQuestions({userId})")
          .Skip(fromLine).Take(amount)
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