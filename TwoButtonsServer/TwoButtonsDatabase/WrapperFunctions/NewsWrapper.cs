using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class NewsWrapper
    {
        public static bool TryGetNews(TwoButtonsContext db, int userId, int amount, out IEnumerable<NewsDb> news)
        {
            
                try
                {
                    news = db.NewsDb.FromSql($"getNews {userId}, {amount}");

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                news = new List<NewsDb>();
                return false;
            
        }
    }
}