using System;
using Microsoft.EntityFrameworkCore;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class QuestionWrapper
    {

        public static bool TryAddQuestion(TwoButtonsContext db, int userId, string condition, string backgroundImageLink, int anonymity, int audience, int questionType, int standartPictureId, string firstOption, string secondOption)
        {
            var askedTime = DateTime.Now;
            try
            {
                db.Database.ExecuteSqlCommand($"addQuestion {userId}, {condition}, {backgroundImageLink}, {anonymity}, {questionType}, {standartPictureId}, {firstOption}, {secondOption}, {askedTime}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool TryUpdateQuestionBackground(TwoButtonsContext db, int questionId, string backgroundImageLink)
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
    }
}