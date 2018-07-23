using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;

namespace DataGenerator.ScriptsGenerators.DirectInsertion
{
  public class StatisticsGenerator
  {
    private readonly string _db = "TwoButtons";

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string SwitchIdentityInsert(bool isEnable)
    {
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Statistics]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return
        "INSERT INTO[dbo].[Statistics] ([userID], [publicAskedQuestions], [askedQuestions], [publicAnsweredQuestions], [answeredQuestions], [seenQuestions], [publicFavoriteQuestions], [favoriteQuestions], [commentsWritten]," +
        "[userQuestionsShows], [userQuestionsAnswers], [followers], [followTo], [questionsCommentsGot],[questionsLikesGot], [questionsDislikesGot], [commentsLikesGot], [commentsDislikesGot], [questionsLikesMade]," +
        "[questionsDislikesMade], [commentsLikesMade], [commentsDislikesMade]) VALUES" + Environment.NewLine;
    }

    private string GetInsertionStatisticsLine(StatisticsEntity statistics)
    {
      return $"(" + $"{statistics.UserId}, " + $"{statistics.PublicAnsweredQuestions}, " +
             $"{statistics.AskedQuestions}, " + $"{statistics.PublicAnsweredQuestions}, " +
             $"{statistics.AnsweredQuestions}, " + $"{statistics.SeenQuestions}, " +
             $"{statistics.PublicFavoriteQuestions}, " + $"{statistics.FavoriteQuestions}, " +
             $"{statistics.CommentsWritten}, " + $"{statistics.UserQuestionsShows}, " +
             $"{statistics.UserQuestionsAnswers}, " + $"{statistics.Followers}, " +
             $"{statistics.FollowTo}, " + $"{statistics.QuestionsCommentsGot}, " +
             $"{statistics.QuestionsLikesGot}, " + $"{statistics.CommentsDislikesGot}, " +
             $"{statistics.CommentsLikesGot}, " + $"{statistics.CommentsDislikesGot}, " +
             $"{statistics.QuestionsLikesMade}, " + $"{statistics.QuestionsDislikesMade}, " +
             $"{statistics.CommentsLikesMade}, " + $"{statistics.CommentsDislikesMade}" + $")";
    }

    private string GetInsertionStatisticssLine(IList<StatisticsEntity> statisticss)
    {
      var result = new StringBuilder();
      for (var i = 0; i < statisticss.Count - 1; i++)
        result.Append(GetInsertionStatisticsLine(statisticss[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionStatisticsLine(statisticss[statisticss.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<StatisticsEntity> statisticss)
    {
      var result = new StringBuilder();
      var times = statisticss.Count < 1000 ? 1 : statisticss.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var cstatisticssIter = statisticss.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Statistics] NOCHECK CONSTRAINT FK_STATISTICS_USER");
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(true));
        result.Append(GetGo());
        result.Append(GetInsertInit());
        result.Append(GetInsertionStatisticssLine(cstatisticssIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(false));
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Statistics] NOCHECK CONSTRAINT FK_STATISTICS_USER");
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}