using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class QuestionWrapper
    {

        public static bool TrySaveFeedback(TwoButtonsContext db, int userId, int questionId, int feedback)
        {
           

               
                var answered = DateTime.Now;

                try
                {
                    db.Database.ExecuteSqlCommand(
                        $"insertFeedback {userId}, {questionId}, {feedback}, {answered}");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            

        }

        public static bool TrySaveFavorites(TwoButtonsContext db, int userId, int questionId, int inFavorites)
        {
           


                var added = DateTime.Now;

                try
                {
                    db.Database.ExecuteSqlCommand($"insertFavorites {userId}, {questionId}, {inFavorites}, {added}");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            
        }

        public static bool TrySaveAnswer(TwoButtonsContext db, int userId, int questionId, int answer)
        {
           
                var answered = DateTime.Now;

                try
                {
                    db.Database.ExecuteSqlCommand($"insertAnswer {userId}, {questionId}, {answer}, {answered}");
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
