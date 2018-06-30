using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsQuestionBaseDb : QuestionBaseDb
  {
    [Column("Sex")]
    public SexType SexType { get; set; }
    public int AnsweredFollowTo { get; set; }
  }
}