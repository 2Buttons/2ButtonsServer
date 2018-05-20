using System;
using System.Linq.Expressions;
using CommonLibraries;
using QuestionsData.Entities;

namespace QuestionsServer.Extensions
{
  public static class QuestionBaseDbExtentions
  {
    public static Expression<Func<T, object>> ToPredicate<T>(this SortType type) where T: QuestionBaseDb, new()
    {
      switch (type)
      {
        case SortType.Raiting:
          return x => x.Likes-x.Dislikes;
        case SortType.ShowsAmount:
          return x => x.Shows;
        case SortType.DateTime:
        default:
          return x => x.QuestionAddDate;
      }
    }

    public static Type ToValueType(this SortType type)

    {
      switch (type)
      {
        case SortType.Raiting:
        case SortType.ShowsAmount:
          return typeof(int);
        case SortType.DateTime:
        default:
          return typeof(DateTime);
      }
    }
  }
}