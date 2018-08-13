using System.Collections.Generic;
using QuestionsData.Queries;

namespace QuestionsData
{
  public class QuestionCompare : IEqualityComparer<QuestionBaseDb>
  {
    public bool Equals(QuestionBaseDb x, QuestionBaseDb y)
    {
      return x.Condition == y.Condition && x.UserId == y.UserId;
    }

    public int GetHashCode(QuestionBaseDb obj)
    {
      return obj.UserId ^ obj.Condition.GetHashCode() ^ 17;
    }
  }
}