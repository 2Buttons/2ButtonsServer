using System;
using System.Collections.Generic;
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
      const string setIdLine = "SET IDENTITY_INSERT [dbo].[Question]";
      return isEnable ? setIdLine + " ON" : setIdLine + " OFF";
    }

    private string GetInsertInit()
    {
      return
        "INSERT INTO [dbo].[Question] ([questionID], [userID], [condition], [anonymity], [audience], [questionType], [questionAddDate]," +
        " [backbroundImageLink], [shows], [likes], [dislikes], [deleted]) VALUES" + Environment.NewLine;
    }

    private string GetUpdatingInit()
    {
      return "UPDATE [dbo].[Question] SET" + Environment.NewLine;
    }

    private string GetInsertionQuestionLine(QuestionEntity question)
    {
      var anonymyty = question.IsAnonimoty ? 1 : 0;
      return
        $"({question.QuestionId}, {question.UserId}, '{question.Condition}', {anonymyty}, {(int) question.AudienceType}, {(int) question.QuestionType}, '{question.QuestionAddDate}, '{question.BackgroundImageLink}', {question.Shows}, {question.Likes}, {question.Dislikes}, {0})";
    }

    private string GetInsertionQuestionsLine(IList<QuestionEntity> questions)
    {
      var result = new StringBuilder();
      for (var i = 0; i < questions.Count - 1; i++)
        result.Append(GetInsertionQuestionLine(questions[i]) + "," + Environment.NewLine);
      result.Append(GetInsertionQuestionLine(questions[questions.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<QuestionEntity> cities)
    {
      var result = new StringBuilder();
      result.Append(GetUsingDb());
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(true));
      result.Append(GetGo());
      result.Append(GetInsertInit());
      result.Append(GetInsertionQuestionsLine(cities));
      result.Append(GetGo());
      result.Append(SwitchIdentityInsert(false));
      result.Append(GetGo());
      return result.ToString();
    }
  }
}