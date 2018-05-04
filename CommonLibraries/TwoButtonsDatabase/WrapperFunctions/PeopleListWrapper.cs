using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

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

        public static bool TryGetRecommendedFromFollows(TwoButtonsContext db, int userId, string searchedLogin,
            out IEnumerable<RecommendedFromFollowsDb> recommendedFromFollows)
        {
            
                try
                {
                    recommendedFromFollows = db.RecommendedFromFollowsDb
                        .FromSql($"select * from dbo.getRecommendedFromFollows({userId}, {searchedLogin})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                recommendedFromFollows = new List<RecommendedFromFollowsDb>();
                return false;
            
        }

        public static bool TryGetRecommendedStrangers(TwoButtonsContext db, int userId, int page, int amount, string searchedLogin,
            out IEnumerable<RecommendedStrangersDb> recommendedStrangers)
        {
            int fromLine = page * amount - amount + 1;
            int toLine = page * amount;
            try
                {
                    recommendedStrangers = db.RecommendedStrangersDb
                        .FromSql($"select * from dbo.getRecommendedStrangers({userId}, {fromLine}, {toLine}, {searchedLogin})").ToList();
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