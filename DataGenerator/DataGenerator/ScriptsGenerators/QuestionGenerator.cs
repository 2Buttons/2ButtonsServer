using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.Entities;

namespace DataGenerator.ScriptsGenerators
{
  public class QuestionGenerator
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Questions]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return
        "INSERT INTO [dbo].[Questions] ([questionID], [userID], [condition], [IsAnonymous], [AudienceType], [QuestionType][QuestionType]," +
        " [QuestionType], [LikesCount], [DislikesCount], [AddedDate], [IsDeleted]) VALUES" + Environment.NewLine;
    }

    private string GetUpdatingInit()
    {
      return "UPDATE [dbo].[Question] SET" + Environment.NewLine;
    }

    private string GetInsertionQuestionLine(QuestionEntity question)
    {
      var anonymyty = question.IsAnonimoty ? 1 : 0;
      return
        $"({question.QuestionId}, {question.UserId}, N'{question.Condition}', {anonymyty}, {(int) question.AudienceType}, {(int) question.QuestionType}, N'{question.OriginalBackgroundUrl}', {question.LikesCount}, {question.DislikesCount}, '{question.QuestionAddDate:u}', {0})";
    }

    private string GetInsertionQuestionsLine(IList<QuestionEntity> questions)
    {
      var result = new StringBuilder();
      for (var i = 0; i < questions.Count - 1; i++)
        result.Append(GetInsertionQuestionLine(questions[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionQuestionLine(questions[questions.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<QuestionEntity> questions)
    {
      var result = new StringBuilder();
      var times = questions.Count < 1000 ? 1 : questions.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var questionsIter = questions.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Questions] NOCHECK CONSTRAINT FK_QUESTION_USER");
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(true));
        result.Append(GetGo());
        result.Append(GetInsertInit());
        result.Append(GetInsertionQuestionsLine(questionsIter));
        result.Append(GetGo());
        result.Append(SwitchIdentityInsert(false));
        result.Append(GetGo());
        result.Append("ALTER TABLE [dbo].[Questions] NOCHECK CONSTRAINT FK_QUESTION_USER");
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}