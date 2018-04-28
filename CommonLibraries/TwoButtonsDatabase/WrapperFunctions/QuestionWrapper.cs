﻿using System;
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

        public static bool TryAddQuestion(TwoButtonsContext db, int userId, string condition, string backgroundImageLink, int anonymity, int audience, int questionType, string firstOption, string secondOption,  out int questionId)
        {
            var questionAddDate = DateTime.UtcNow;

            var questionIdDb = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            try
            {
                db.Database.ExecuteSqlCommand($"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {audience}, {questionType}, {questionAddDate}, {firstOption}, {secondOption}, {questionIdDb} OUT");
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

        public static bool TryDeleteQuestion(TwoButtonsContext db, int questionId)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"deleteQuestion {questionId}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static bool TryAddAnswer(TwoButtonsContext db, int userId, int questionId, int answer, int yourFeedback)
        {
            var anwserAddDate = DateTime.UtcNow;
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

        public static bool TryGetAskedQuestions(TwoButtonsContext db, int userId, int getUserId, int page, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 1;
            int fromLine = page * amount;
            int toLine = (page + 1) * amount;
            try
            {
                userAskedQuestions = db.UserAskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {fromLine}, {toLine}, {isAnonimus})")
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

        public static bool TryGetLikedQuestions(TwoButtonsContext db, int userId, int getUserId, int page, int amount, out IEnumerable<UserAskedQuestionDb> userAskedQuestions)
        {
            var isAnonimus = 1;
            int fromLine = page * amount;
            int toLine = (page + 1) * amount;
            try
            {
                userAskedQuestions = db.UserAskedQuestionsDb
                    .FromSql($"select * from dbo.getUserAskedQuestions({userId}, {getUserId}, {fromLine}, {toLine}, {isAnonimus})")
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