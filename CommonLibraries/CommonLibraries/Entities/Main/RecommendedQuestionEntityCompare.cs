using System.Collections.Generic;

namespace CommonLibraries.Entities.Main
{
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