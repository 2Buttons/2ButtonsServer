using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsQuestionBaseDb : QuestionBaseDb
  {
    public int AnsweredFollowingsCount { get; set; }
  }
}