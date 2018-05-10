﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class ResultsWrapper
  {
    public static bool TryGetPhotos(TwoButtonsContext db, int userId, int questionId, int answer, int amount,
      int minAge,
      int maxAge, int sex, string city, out IEnumerable<PhotoDb> photos)
    {
      try
      {
        photos = db.PhotoDb
          .FromSql($"select * from dbo.getPhotos({userId}, {questionId}, {answer}, {amount}, {minAge}, {maxAge}, {sex}, {city})")
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      photos = new List<PhotoDb>();
      return false;
    }

    public static bool TryGetAnsweredList(TwoButtonsContext db, int userId, int questionId, int page, int amount, AnswerType answerType,
      int minAge, int maxAge, SexType sexType, string searchedLogin, out IEnumerable<AnsweredListDb> answeredList)
    {
      var fromLine = page * amount - amount;

      try
      {
        answeredList = db.AnsweredListDb
          .FromSql($"select * from dbo.getAnsweredList({userId}, {questionId},   {answerType}, {minAge}, {maxAge}, {sexType}, {searchedLogin})")
          .Skip(fromLine).Take(amount)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      answeredList = new List<AnsweredListDb>();
      return false;
    }
  }
}