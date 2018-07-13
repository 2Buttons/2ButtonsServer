using System;
using System.Collections.Generic;
using QuestionsServer.ViewModels.OutputParameters.NewsQuestions;

namespace QuestionsServer.Infrastructure
{
  public class NewsQuestionBaseCompare : IEqualityComparer<NewsQuestionBaseViewModel>
  {
    public bool Equals(NewsQuestionBaseViewModel x, NewsQuestionBaseViewModel y)
    {
      return x.Condition == y.Condition && x.Author.UserId == y.Author.UserId;
    }

    public int GetHashCode(NewsQuestionBaseViewModel obj)
    {
      return obj.Author.UserId ^ obj.Condition.GetHashCode() ^ 17;
    }
  }
}