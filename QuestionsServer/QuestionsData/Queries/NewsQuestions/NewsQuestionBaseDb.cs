using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsQuestionBaseDb : QuestionBaseDb
  {
    public SexType Sex { get; set; }
    public int AnsweredFollowTo { get; set; }
  }
}