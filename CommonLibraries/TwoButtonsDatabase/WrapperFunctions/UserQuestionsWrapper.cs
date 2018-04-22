using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.UserQuestions;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class UserQuestionsWrapper
    {
        public static bool TryAddRecommendedQuestion(TwoButtonsContext db, int userToId, int userFromId, int questionId)
        {
            var recommendedDate = DateTime.Now;
            try
            {
                db.Database.ExecuteSqlCommand($"addTag {userToId}, {userFromId}, {questionId}, {recommendedDate}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }



        public static bool TryGetUserAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 0;
            try
            {
                userAskedQuestions = db.UserAskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userAskedQuestions = new List<UserAskedQuestionDb>();
            return false;
        }

        public static bool TryGetUserAnsweredQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount, out IEnumerable<UserAnsweredQuestionDb> userAnsweredQuestions)
        {
            var isAnonimus = 0;
            try
            {
                userAnsweredQuestions = db.UserAnsweredQuestionsDb
                    .FromSql(
                        $"select * from dbo.getUserAnsweredQuestions({userId}, {pageUserId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userAnsweredQuestions = new List<UserAnsweredQuestionDb>();
            return false;
        }

        public static bool TryGetUserFavoriteQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount, out IEnumerable<UserFavouriteQuestionDb> userFavouriteQuestions)
        {
            var isAnonimus = 0;
            try
            {
                userFavouriteQuestions = db.UserFavouriteQuestionsDb
                    .FromSql(
                        $"select * from dbo.getUserFavoriteQuestions({userId}, {pageUserId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userFavouriteQuestions = new List<UserFavouriteQuestionDb>();
            return false;
        }

        public static bool TryGetUserCommentedQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount,
            out IEnumerable<UserCommentedQuestionDb> userCommentedQuestions)
        {
            try
            {
                userCommentedQuestions = db.UserCommentedQuestionsDb
                    .FromSql($"select * from dbo.getUserCommentedQuestions({userId}, {pageUserId}, {amount})").ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userCommentedQuestions = new List<UserCommentedQuestionDb>();
            return false;
        }

        public static bool TryGeTopQuestions(TwoButtonsContext db, int userId, bool isOnlyNew, int amount, DateTime topAfterDate,
            out IEnumerable<TopQuestionDb> topQuestions)
        {
            try
            {
                topQuestions = db.TopQuestionsDb
                    .FromSql($"select * from dbo.getTop({userId}, {amount}, {topAfterDate}, {isOnlyNew})").ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            topQuestions = new List<TopQuestionDb>();
            return false;
        }



        public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount, out IEnumerable<AskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 1;
            try
            {
                userAskedQuestions = db.AskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {pageUserId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userAskedQuestions = new List<AskedQuestionDb>();
            return false;
        }

        public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int pageUserId, int amount, out IEnumerable<LikedQuestionDb> userAnsweredQuestions)
        {
            var isAnonimus = 1;
            try
            {
                userAnsweredQuestions = db.LikedQuestionsDb
                    .FromSql(
                        $"select * from dbo.getUserLikedQuestions({userId}, {pageUserId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userAnsweredQuestions = new List<LikedQuestionDb>();
            return false;
        }

        public static bool TryGetSavedQuestions(TwoButtonsContext db, int userId, int amount, out IEnumerable<SavedQuestionDb> userFavouriteQuestions)
        {
            var isAnonimus = 1;
            try
            {
                userFavouriteQuestions = db.SavedQuestionsDb
                    .FromSql(
                        $"select * from dbo.getUserFavoriteQuestions({userId}, {amount}, {isAnonimus})")
                    .ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userFavouriteQuestions = new List<SavedQuestionDb>();
            return false;
        }
    }
}