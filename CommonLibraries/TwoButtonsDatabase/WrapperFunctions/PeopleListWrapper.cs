using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Recommended;

namespace TwoButtonsDatabase.WrapperFunctions
{
  public static class PeopleListWrapper
  {
    public static bool TryGetRecommendedFromContacts(TwoButtonsContext db, int userId, string searchedLogin,
      out IEnumerable<RecommendedFromContactsDb> recommendedFromContacts)
    {
      try
      {
        recommendedFromContacts = db.RecommendedFromContactsDb
          .FromSql($"select * from dbo.getRecommendedFromContacts({userId}, {searchedLogin})").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedFromContacts = new List<RecommendedFromContactsDb>();
      return false;
    }

    

    public static bool TryGetRecommendedStrangers(TwoButtonsContext db, int userId, int offset, int count, string searchedLogin,
      out IEnumerable<RecommendedStrangersDb> recommendedStrangers)
    {
      

      try
      {
        recommendedStrangers = db.RecommendedStrangersDb
          .FromSql($"select * from dbo.getRecommendedStrangers({userId},   {searchedLogin})")
          .Skip(offset).Take(count)
          .ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      recommendedStrangers = new List<RecommendedStrangersDb>();
      return false;
    }
  }
}