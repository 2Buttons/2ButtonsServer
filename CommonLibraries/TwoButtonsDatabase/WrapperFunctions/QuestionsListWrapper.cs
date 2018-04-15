using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class QuestionsListWrapper
    {
        public static bool TryGetUserAskedQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, bool showAnonimous, out IEnumerable<UserAskedQuestionsDb> userAskedQuestions)
        {

           
                var isAnonimus = showAnonimous ? 1 : 0;
                try
                {
                    userAskedQuestions = db.UserAskedQuestionsDb
                        .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {amount}, {isAnonimus})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userAskedQuestions = new List<UserAskedQuestionsDb>();
                return false;
            

           
        }

        public static bool TryGetUserAnsweredQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, bool showAnonimous, out IEnumerable<UserAnsweredQuestionsDb> userAnsweredQuestions)
        {

            
                var isAnonimus = showAnonimous ? 1 : 0;
                try
                {
                    userAnsweredQuestions = db.UserAnsweredQuestionsDb
                        .FromSql($"select * from dbo.getUserAnsweredQuestions({userId}, {getUserId}, {amount}, {isAnonimus})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userAnsweredQuestions = new List<UserAnsweredQuestionsDb>();
                return false;
            
            
        }

        public static bool TryGetUserFavoriteQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, bool showAnonimous, out IEnumerable<UserFavouriteQuestionsDb> userFavouriteQuestions)
        {

           
                var isAnonimus = showAnonimous ? 1 : 0;
                try
                {
                    userFavouriteQuestions = db.UserFavouriteQuestionsDb
                        .FromSql($"select * from dbo.getUserFavoriteQuestions({userId}, {getUserId}, {amount}, {isAnonimus})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userFavouriteQuestions = new List<UserFavouriteQuestionsDb>();
                return false;
            
            
        }

        public static bool TryGetUserCommentedQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, out IEnumerable<UserCommentedQuestionsDb> userCommentedQuestions)
        {
            
                try
                {
                    userCommentedQuestions = db.UserCommentedQuestionsDb
                        .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {getUserId}, {amount})").ToList();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userCommentedQuestions = new List<UserCommentedQuestionsDb>();
                return false;
            

        }
    }
}
