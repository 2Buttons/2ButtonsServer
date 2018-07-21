using System;
using System.Linq.Expressions;
using CommonLibraries;
using QuestionsData.Entities;
using QuestionsData.Queries;

namespace QuestionsServer.Extensions
{
  public static class SortTypeExtentions
  {
    public static Expression<Func<T, object>> ToPredicate<T>(this SortType type) where T: QuestionBaseDb, new()
    {
      switch (type)
      {
        case SortType.Raiting:
          return x => x.LikesCount-x.DislikesCount;
        case SortType.AnswersCount:
          return x => x.FirstAnswersCount+x.SecondAnswersCount;
        case SortType.DateTime:
        default:
          return x => x.AddedDate;
      }
    }


    public static Type ToValueType(this SortType type)

    {
      switch (type)
      {
        case SortType.Raiting:
        case SortType.AnswersCount:
          return typeof(int);
        case SortType.DateTime:
        default:
          return typeof(DateTime);
      }
    }
  }
}