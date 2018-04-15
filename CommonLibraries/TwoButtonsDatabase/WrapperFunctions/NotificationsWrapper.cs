using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class NotificationsWrapper
    {
        public static bool TryGetNewFollowers(TwoButtonsContext db, int userId, out IEnumerable<NewFollowersDb> newFollowers)
        {
            try
            {
                newFollowers = db.NewFollowersDb
                    .FromSql($"select * from dbo.getNewFollowers({userId})").ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            newFollowers = new List<NewFollowersDb>();
            return false;
        }

        public static bool TryGetNewRecommendedQuestions(TwoButtonsContext db, int userId, out IEnumerable<NewRecommendedQuestionDb> newRecommendedQuestion)
        {

            try
            {
                newRecommendedQuestion = db.NewRecommendedQuestionDb
                    .FromSql($"select * from dbo.getNewRecommendedQuestions({userId})").ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            newRecommendedQuestion = new List<NewRecommendedQuestionDb>();
            return false;
        }

        public static bool TryUpdateNotsDate(TwoButtonsContext db, int userId)
        {
            
                try
                {
                    var newLastNots = DateTime.Now;
                    db.Database.ExecuteSqlCommand($"updateNotsDate {userId}, {newLastNots}");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            
        }
    }
}
