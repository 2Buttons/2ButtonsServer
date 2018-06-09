using System;
using System.Collections.Generic;
using QuestionsServer.ViewModels.OutputParameters.NewsQuestions;

namespace QuestionsServer.Infrastructure
{
  public class NewsQuestionBaseCompare : IEqualityComparer<NewsQuestionBaseViewModel>
  {
    public bool Equals(NewsQuestionBaseViewModel x, NewsQuestionBaseViewModel y)
    {
      return x.Condition == y.Condition && x.UserId == y.UserId;
    }

    public int GetHashCode(NewsQuestionBaseViewModel obj)
    {
      return obj.UserId ^ obj.Condition.GetHashCode() ^ 17;
    }
  }
}