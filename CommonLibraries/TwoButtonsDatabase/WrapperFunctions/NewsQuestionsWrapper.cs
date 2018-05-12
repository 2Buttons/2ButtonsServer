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
    public static bool TryGetNewsAskedQuestions(TwoButtonsContext db, int userId, int offset, int count,
      out IEnumerable<NewsAskedQuestionsDb> newsAskedQuestions)
    {
      

      try
      {
        newsAskedQuestions = db.NewsAskedQuestionsDb
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

    public static bool TryGetNewsAnsweredQuestions(TwoButtonsContext db, int userId, int offset, int count,
      out IEnumerable<NewsAnsweredQuestionsDb> newsAnsweredQuestions)
    {
      

      try
      {
        newsAnsweredQuestions = db.NewsAnsweredQuestionsDb
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

    public static bool TryGetNewsFavoriteQuestions(TwoButtonsContext db, int userId, int offset, int count,
      out IEnumerable<NewsFavoriteQuestionsDb> newsFavoriteQuestions)
    {
      

      try
      {
        newsFavoriteQuestions = db.NewsFavoriteQuestionsDb
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

    public static bool TryGetNewsCommentedQuestions(TwoButtonsContext db, int userId, int offset, int count,
      out IEnumerable<NewsCommentedQuestionsDb> newsCommentedQuestions)
    {
      

      try
      {
        newsCommentedQuestions = db.NewsCommentedQuestionsDb
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

    public static bool TryGetNewsRecommendedQuestions(TwoButtonsContext db, int userId, int offset, int count,
      out IEnumerable<NewsRecommendedQuestionDb> newRecommendedQuestion)
    {
      

      try
      {
        newRecommendedQuestion = db.NewsRecommendedQuestionDb
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