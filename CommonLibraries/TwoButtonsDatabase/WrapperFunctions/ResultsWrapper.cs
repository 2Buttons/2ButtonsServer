using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class ResultsWrapper
    {
        public static bool TryGetResults(TwoButtonsContext db, int userId, int questionId, int minAge, int maxAge, int sex, out IEnumerable<ResultsDb> results)
        {
           
                try
                {
                    results = db.ResultsDb
                        .FromSql($"select * from dbo.getResults({userId}, {questionId}, {minAge}, {maxAge}, {sex})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                results = new List<ResultsDb>();
                return false;
            


        }

        public static bool TryGetPhotos(TwoButtonsContext db, int userId, int questionId, int amount, int minAge, int maxAge, int sex, out IEnumerable<PhotoDb> photos)
        {

            
                try
                {
                    photos = db.PhotoDb
                        .FromSql($"select * from dbo.getPhotos({userId}, {questionId}, {minAge}, {maxAge}, {sex})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                photos = new List<PhotoDb>();
                return false;
            
           
        }

        public static bool TryGetAnsweredList(TwoButtonsContext db, int userId, int questionId, int amount, int answer, int minAge, int maxAge, int sex, String search, out IEnumerable<AnsweredListDb> answeredList)
        {

           
                try
                {
                    answeredList = db.AnsweredListDb
                        .FromSql($"select * from dbo.getAnsweredList({userId}, {questionId}, {minAge}, {maxAge}, {sex})").ToList();
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
