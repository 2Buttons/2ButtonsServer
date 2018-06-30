using System.Collections.Generic;
using QuestionsData.Queries;

namespace QuestionsData.Entities
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

  public class RecommendedQuestionEntityCompare : IEqualityComparer<RecommendedQuestionEntity>
  {
    public bool Equals(RecommendedQuestionEntity x, RecommendedQuestionEntity y)
    {
      return x.QuestionId == y.QuestionId && x.UserFromId == y.UserFromId && x.UserToId == y.UserToId;
    }

    public int GetHashCode(RecommendedQuestionEntity obj)
    {
      return obj.QuestionId ^ obj.UserFromId^ obj.UserToId ^ 17;
    }
  }
}