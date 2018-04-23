using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.UserQuestions;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class QuestionWrapper
    {

        public static bool TryAddQuestion(TwoButtonsContext db, int userId, string condition, int anonymity, int audience, int questionType, int standartPictureId, string firstOption, string secondOption,  string backgroundImageLink, out int questionId)
        {
            var questionAddDate = DateTime.Now;

            var questionIdDb = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            try
            {
                db.Database.ExecuteSqlCommand($"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {standartPictureId}, {firstOption}, {secondOption}, {questionIdDb} OUT");
                questionId = (int)questionIdDb.Value;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            questionId = -1;
            return false;
        }

        public static bool TryAddAnswer(TwoButtonsContext db, int userId, int questionId, string answer, int yourFeedback)
        {
            var anwserAddDate = DateTime.Now;
            try
            {
                db.Database.ExecuteSqlCommand($"addAnswer {userId}, {questionId}, {answer}, {yourFeedback}, {anwserAddDate}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool TryUpdateQuestionBackgroundLink(TwoButtonsContext db, int questionId, string backgroundImageLink)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"updateQuestionBackground {questionId}, {backgroundImageLink}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool TrySaveFeedback(TwoButtonsContext db, int userId, int questionId, int feedback)
        {
            var answered = DateTime.Now;

            try
            {
                db.Database.ExecuteSqlCommand($"insertFeedback {userId}, {questionId}, {feedback}, {answered}");
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

        public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 1;
            try
            {
                userAskedQuestions = db.UserAskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {amount}, {isAnonimus})")
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

        public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int getUserId, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 1;
            try
            {
                userAskedQuestions = db.UserAskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {amount}, {isAnonimus})")
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
    }
}